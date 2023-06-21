import {FetchHttpPostJson} from "/js/Fetcher.js";
import {BuildNextStepButton} from "/js/CourseView/NextStep.js";
import {AddWrongAnswerMarker, AddCorrectAnswerMarker} from "/js/CourseView/CorrectnessMarker.js";
import {CreateAlertSuccess, CreateAlertError} from "/js/Alert.js";

const stepIds = {
    Quiz: "quiz",
    QuestionText: "questionText",
    ItemParent: "itemContainer"
};

export function BuildCourseQuiz(data) {
    const title = buildTitle(data);
    const quiz = buildQuiz(data);
    const checkChoice = buildCheckChoice();
    return [title, quiz, checkChoice];
}

function buildTitle(data) {
    const title = document.createElement("div");
    title.classList.add("title");
    title.innerText = data["title"];
    return title;
}

function buildQuiz(data) {
    const quiz = document.createElement("div");
    quiz.style.textAlign = "center";
    quiz.dataset.id = data["itemId"];
    quiz.id = stepIds.Quiz;
    
    const question = document.createElement("div");
    
    const questionText = document.createElement("h2");
    questionText.id = stepIds.QuestionText;
    questionText.innerHTML = data["questionText"];
    questionText.classList.add("question-text");
    
    question.appendChild(questionText);

    quiz.appendChild(question);
    const dataQuizChoices = data["possibleAnswers"];

    for (const element of dataQuizChoices) {
        const quizChoice = buildQuizChoice(element);
        quiz.appendChild(quizChoice);
    }
    return quiz;
}

function buildQuizChoice(dataChoice) {
    const parent = document.createElement("div");

    const choiceInput = document.createElement("input");

    const choiceId = dataChoice["quizChoiceId"];
    const htmlElementId = `choice${choiceId}`;
    choiceInput.setAttribute("type", "checkbox");
    choiceInput.setAttribute("id", htmlElementId);
    choiceInput.dataset["choiceId"] = choiceId;

    const label = document.createElement("label");
    label.setAttribute("for", htmlElementId);
    label.innerText = dataChoice["choiceText"];
    label.classList.add("choice-text");

    parent.appendChild(choiceInput);
    parent.appendChild(label);

    return parent;
}

function buildCheckChoice() {
    const button = document.createElement("button");
    button.innerText = "Verifica raspunsul";
    button.classList.add("CheckAnswer");
    button.addEventListener("click", buttonCheckChoiceClick);
    return button;
}

function buttonCheckChoiceClick() {
    const quiz = document.getElementById(stepIds.Quiz);
    const requestBody = {
        quizId: quiz.dataset.id,
        quizChoiceIds: getUserChoiceIds()
    };
    console.log(requestBody);
    FetchHttpPostJson("/api/quiz/validate", requestBody)
        .then(apiResponse => {
             const isValid = apiResponse["isValid"];
             if (isValid !== true) {
                 InvalidAnswerCallback("Ai raspuns gresit! Mai incearca o data!");
                 return;
             }
             RightAnswerCallback();
        })
        .catch(err => {
            console.log(err);
            InvalidAnswerCallback("Raspunsul nu a putut fi procesat");
        });
}

function RightAnswerCallback() {
    const questionTextElement = document.getElementById("questionText");
    let text = questionTextElement.innerText;
    questionTextElement.innerHTML = AddCorrectAnswerMarker(text);
    
    const buttonNextStep = BuildNextStepButton();
    const isButtonNextStepAlreadyPresent = document.getElementById(buttonNextStep.id) !== null;
    if (isButtonNextStepAlreadyPresent) {
        return;
    }
    const itemParent = document.getElementById(stepIds.ItemParent);
    const emptyParagraph = () => document.createElement("p");
    itemParent.appendChild(emptyParagraph());
    itemParent.appendChild(buttonNextStep);

    const alert = CreateAlertSuccess("Ai raspuns corect !");
    const quiz = document.getElementById(stepIds.Quiz);
    quiz.appendChild(alert);
}

function InvalidAnswerCallback(message) {
    const questionTextElement = document.getElementById("questionText");
    let text = questionTextElement.innerText;
    questionTextElement.innerHTML = AddWrongAnswerMarker(text);

    const alert = CreateAlertError(message);
    const quiz = document.getElementById(stepIds.Quiz);
    quiz.appendChild(alert);
}

function getUserChoiceIds() {
    let quizChoiceUserIds = [];
    let inputNodeList = document.querySelectorAll("input");
    for (const element of inputNodeList) {
        const input = element;
        if (input.checked === true) {
            quizChoiceUserIds.push(input.dataset.choiceId);
        }
    }
    return quizChoiceUserIds;
}