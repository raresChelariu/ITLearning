import { FetchHttpPost } from "/js/fetcher.js";

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

    const question = document.createElement("div");
    question.classList.add("intrebare");

    const questionText = document.createElement("h2");
    questionText.innerText = data["questionText"];
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

function buildCheckChoice(data) {
    const button = document.createElement("button");
    button.innerText = "Verifica raspunsul";
    button.classList.add("QuizCheckChoice");
    button.addEventListener('click', () => {
        const requestBody = {
            quizId: data["itemId"],
            quizChoiceIds: getUserChoices()
        };
        console.log(requestBody);
        FetchHttpPost("/api/quiz/validate", requestBody)
            .then(x => {
                // TODO Handle API Response
                console.log("API RESPONSE VALIDATE")
                console.log(x);
            });
    })
    return button;
}

function getUserChoices() {
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