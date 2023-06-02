export function BuildCourseQuiz(data)
{
    const title = buildTitle(data);
    const quiz = buildQuiz(data);
    return [title, quiz];
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
    choiceInput.setAttribute("type", "checkbox");
    choiceInput.setAttribute("id", choiceId);
    choiceInput.dataset["choiceId"] = choiceId;
    
    const label = document.createElement("label");
    label.setAttribute("for", choiceId);
    label.innerText = dataChoice["choiceText"];
    
    parent.appendChild(choiceInput);
    parent.appendChild(label);
    
    return parent;
}