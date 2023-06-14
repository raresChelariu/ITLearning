import {FetchHttpPostJson} from "/js/Fetcher.js";
import {AddWrongAnswerMarker, AddCorrectAnswerMarker} from "/js/CourseView/CorrectnessMarker.js";
import {CreateAlertSuccess, CreateAlertError} from "/js/Alert.js";
import {BuildNextStepButton} from "/js/CourseView/NextStep.js"

export function BuildCourseSqlQuiz(data) {
    const title = buildQuizTitle(data);
    const quiz = buildQuiz(data);
    const checkChoice = buildCheckChoice(data);
    return [title, quiz, checkChoice, BuildNextStepButton()];
}

const pageIds = {
    Quiz: "quiz",
    QuestionText: "questionText",
    UserInput: "userInput"
};

function buildCheckChoice() {
    const button = document.createElement("button");
    button.innerText = "Verifica raspunsul";
    button.classList.add("QuizCheckChoice");
    button.addEventListener('click', buttonCheckChoiceClick);
    return button;
}

function buildQuizTitle(data) {
    const title = document.createElement("div");
    title.classList.add("title");
    title.innerText = data["title"];
    return title;
}

function buildQuiz(data) {
    const quiz = document.createElement("div");
    quiz.classList.add("Quizz");
    quiz.dataset.id = data["itemId"];
    quiz.id = pageIds.Quiz;

    const question = document.createElement("div");
    question.classList.add("intrebare");

    const questionText = document.createElement("h2");
    questionText.id = pageIds.QuestionText;
    questionText.innerHTML = data["questionText"];
    question.appendChild(questionText);

    const userInput = document.createElement("textarea");
    userInput.id = pageIds.UserInput;
    
    quiz.appendChild(question);
    quiz.appendChild(userInput);
    
    return quiz;
}

function buttonCheckChoiceClick() {
    const userInput = document.getElementById(pageIds.UserInput);
    const quiz = document.getElementById(pageIds.Quiz);
    
    const request = {
        sqlQuizId: quiz.dataset.id,
        query: userInput.value
    };
    
    FetchHttpPostJson("/api/sqlquiz/validate", request)
        .then(response => {
            const isValid = response["isValid"];
            if (isValid === true) {
                HandleCorrectResponse(response);
                return;
            }
            HandleWrongResponse(response);
        })
        .catch(err => {
            HandleWrongResponse();
            console.log(err);
        });
}

function HandleCorrectResponse(data) {
    const questionText = document.getElementById(pageIds.QuestionText);
    questionText.innerHTML = AddCorrectAnswerMarker(questionText.innerText);
    const alert = CreateAlertSuccess("Ai raspuns corect !");
    const quiz = document.getElementById(pageIds.Quiz);
    quiz.appendChild(alert);    
}

function HandleWrongResponse(data) {
    const questionText = document.getElementById(pageIds.QuestionText);
    questionText.innerHTML = AddWrongAnswerMarker(questionText.innerText);
    const alert = CreateAlertError(data["errorMessage"]);
    const quiz = document.getElementById(pageIds.Quiz);
    quiz.appendChild(alert);
}