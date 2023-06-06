import {FetchHttpPost} from "/js/Fetcher.js";
import {BuildNextStepButton} from "/js/NextStep.js";

const htmlEntities = {
    rightChoices: {
        htmlEncoding: "&#9989;",
        charCode: 9989
    },
    wrongChoices: {
        htmlEncoding: "&#10060;",
        charCode: 10060
    }
}

export function BuildCourseQuiz(data) {
    const title = buildTitle(data);
    const quiz = buildQuiz(data);
    const checkChoice = buildCheckChoice(data);
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
    quiz.classList.add("Quizz");
    quiz.dataset.id = data["itemId"];
    quiz.id = "quiz";
    
    const question = document.createElement("div");
    question.classList.add("intrebare");

    const questionText = document.createElement("h2");
    questionText.id = "questionText";
    questionText.innerHTML = data["questionText"];
    question.appendChild(questionText);

    quiz.appendChild(question);
    const dataQuizChoices = data["possibleAnswers"];

    for (let i = 0; i < dataQuizChoices.length; i++) {
        const quizChoice = buildQuizChoice(dataQuizChoices[i]);
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

    parent.appendChild(choiceInput);
    parent.appendChild(label);

    return parent;
}

function buildCheckChoice() {
    const button = document.createElement("button");
    button.innerText = "Verifica raspunsul";
    button.classList.add("QuizCheckChoice");
    button.addEventListener('click', buttonCheckChoiceClick);
    return button;
}

function buttonCheckChoiceClick() {
    const quiz = document.getElementById("quiz");
    const requestBody = {
        quizId: quiz.dataset.id,
        quizChoiceIds: getUserChoiceIds()
    };
    console.log(requestBody);
    FetchHttpPost("/api/quiz/validate", requestBody)
        .then(apiResponse => {
             const isValid = apiResponse["isValid"];
             if (isValid !== true) {
                 InvalidAnswerCallback();
                 return;
             }
             RightAnswerCallback();
        })
        .catch(err => {
            console.log(err);
            InvalidAnswerCallback();
        });
}

function RightAnswerCallback() {
    const questionTextElement = document.getElementById("questionText");
    let text = questionTextElement.innerText;
    text = removeHtmlEntitiesFromString(text);
    text = `${htmlEntities.rightChoices.htmlEncoding} ${text}`;
    questionTextElement.innerHTML = text;
    
    const buttonNextStep = BuildNextStepButton();
    const isButtonNextStepAlreadyPresent = document.getElementById(buttonNextStep.id) !== null;
    if (isButtonNextStepAlreadyPresent) {
        return;
    }
    const itemParent = document.getElementById("itemContainer");
    itemParent.appendChild(buttonNextStep);
}

function InvalidAnswerCallback() {
    const questionTextElement = document.getElementById("questionText");
    let text = questionTextElement.innerText;
    text = removeHtmlEntitiesFromString(text);
    text = `${htmlEntities.wrongChoices.htmlEncoding} ${text}`;
    questionTextElement.innerHTML = text;
    alert("Raspunsul este gresit");
}

function removeHtmlEntitiesFromString(text) {
    return text.replaceAll(String.fromCharCode(10060), "").replaceAll(String.fromCharCode(9989), "");
}

function getUserChoiceIds() {
    let quizChoiceUserIds = [];
    let inputNodeList = document.querySelectorAll("input");
    for (let index = 0; index < inputNodeList.length; index++) {
        const input = inputNodeList[index];
        if (input.checked === true) {
            quizChoiceUserIds.push(input.dataset.choiceId);
        }
    }
    return quizChoiceUserIds;
}