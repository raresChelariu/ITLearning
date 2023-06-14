import {FetchHttpPostJson} from "/js/Fetcher.js"
import {AddStepToSummary} from "/js/CourseCreate/CourseCreateSummary.js";

const stepIds = {
    InputQuizTitle: "inputQuizTitle",
    InputQuizQuestion: "inputQuizQuestion",
    InputQueryAnswer: "inputQueryAnswer"
};

export function GetSqlQuizStepBuilder() {
    const labelTitle = createQuizLabelTitle();
    const inputTitle = createQuizInputTitle();

    const labelQuizQuestion = createQuizLabelQuestionText();
    const inputQuizQuestion = createQuizInputQuestion();

    const labelQueryAnswer = createLabelQueryAnswer();
    const inputQueryAnswer = createInputQueryAnswer();

    const buttonAddStep = createButtonAddStep();

    return [labelTitle, inputTitle
        , labelQuizQuestion, inputQuizQuestion
        , labelQueryAnswer, inputQueryAnswer
        , buttonAddStep];
}

function createLabelQueryAnswer() {
    const label = document.createElement("label");
    label.setAttribute("for", stepIds.InputQueryAnswer);
    label.innerText = "Scrie interogarea care reprezinta raspunsul corect";
    return label;
}

function createInputQueryAnswer() {
    const input = document.createElement("input");
    input.id = stepIds.InputQueryAnswer;
    return input;
}

function createQuizLabelTitle() {
    const label = document.createElement("label");
    label.for = stepIds.InputQuizTitle;
    label.innerText = "Titlu Quiz";
    return label;
}

function createQuizInputTitle() {
    const input = document.createElement("input");
    input.id = stepIds.InputQuizTitle;
    input.type = "text";
    input.classList.add("endOfRow");
    return input;
}

function createQuizLabelQuestionText() {
    const label = document.createElement("label");
    label.for = stepIds.InputQuizQuestion;
    label.innerText = "Textul intrebarii quiz-ului";
    return label;
}

function createQuizInputQuestion() {
    const input = document.createElement("input");
    input.id = stepIds.InputQuizQuestion;
    input.type = "text";
    input.classList.add("endOfRow");
    return input;
}

function createButtonAddStep() {
    const button = document.createElement("button");
    button.classList.add("NextStep");
    button.innerText = "Adauga pas";
    button.addEventListener("click", buttonAddStepOnClick);
    return button;
}

function buttonAddStepOnClick() {
    const quizTitle = document.getElementById(stepIds.InputQuizTitle).value;
    const request = {
        courseId: GetCourseId(),
        title: quizTitle,
        questionText: document.getElementById(stepIds.InputQuizQuestion).value,
        expectedQuery: document.getElementById(stepIds.InputQueryAnswer).value
    };
    FetchHttpPostJson("/api/sqlquiz", request)
        .then(response => {
            alert("Quizul SQL a fost adaugat cu succes!");
            AddStepToSummary({
                stepTitle: quizTitle
            });
        })
        .catch(err => {
            alert("Quizul SQL nu a putut fi adaugat!");
            console.log(err);
        });
}

function GetCourseId() {
    const panelCreateCourse = document.getElementById("panelCourseCreate");
    return parseInt(panelCreateCourse.dataset.id);
}
