import {FetchHttpPostJson} from "/js/Fetcher.js"
import {AddStepToSummary} from "/js/CourseCreate/CourseCreateSummary.js";

const stepIds = {
    InputQuizTitle: "inputQuizTitle",
    InputQuizQuestion: "inputQuizQuestion",
    ChoicesContainer: "choicesContainer"
};

export function GetQuizStepBuilder() {
    const labelTitle = createQuizLabelTitle();
    const inputTitle = createQuizInputTitle();
    const labelQuizQuestion = createQuizLabelQuestionText();
    const inputQuizQuestion = createQuizInputQuestion();
    const choicesContainer = createChoicesContainer();
    
    const buttonAddChoice = createButtonAddChoice();
    const buttonAddStep = createButtonAddStep();
    
    return [labelTitle, inputTitle, labelQuizQuestion, 
        inputQuizQuestion, choicesContainer, buttonAddChoice, buttonAddStep];    
}

function createButtonAddStep() {
    const button = document.createElement("button");
    button.classList.add("NextStep");
    button.innerText = "Adauga pas";
    button.addEventListener('click', () => {
        const choicesDto = [];
        const choiceContainer = document.getElementById(stepIds.ChoicesContainer);
        
        for (const choiceDiv of choiceContainer.children) {
            let choiceInput = null;
            for (const choiceComponent of choiceDiv.children) {
                if (choiceComponent.tagName.toLowerCase() === "input") {
                    choiceInput = choiceComponent;
                }
            }
            if (choiceInput !== undefined && choiceInput !== null) {
                let choiceDto = {
                    choiceText: choiceInput.value,
                    isRight: choiceInput.dataset.isright === "true",
                }
                choicesDto.push(choiceDto);    
            }            
        }
        const request = {
            courseId: GetCourseId(),
            quizTitle: document.getElementById(stepIds.InputQuizTitle).value,
            questionText: document.getElementById(stepIds.InputQuizQuestion).value,
            choices: choicesDto 
        };
        
        FetchHttpPostJson("/api/quiz", request)
            .then(() => {
                alert("Pasul Quiz a fost creat cu succes!");
                AddStepToSummary({
                   stepTitle: request.quizTitle 
                });
            })
            .catch(err => {
                alert("Pasul Quiz nu a putut fi creat!");
                console.log(err);
            });
    });
    return button;
}
function createChoicesContainer() {
    const choicesContainer = document.createElement("div");
    choicesContainer.id = stepIds.ChoicesContainer;
    choicesContainer.name = stepIds.ChoicesContainer;
    choicesContainer.dataset.lastchoice = 1 + "";
    choicesContainer.appendChild(createQuizChoicePanel(1));
    return choicesContainer;
}

function createButtonAddChoice() {
    const button = document.createElement("button");
    button.innerText = "Adauga un raspuns";
    button.classList.add("NextStep");
    button.classList.add("endOfRow");
    
    button.addEventListener("click", () => {
        const parentChoices = document.getElementById(stepIds.ChoicesContainer);
        const lastChoiceId = parseInt(parentChoices.dataset.lastchoice);
        const choiceToBeAddedId = lastChoiceId + 1;
        parentChoices.dataset.lastchoice = choiceToBeAddedId + "";
        parentChoices.appendChild(createQuizChoicePanel(choiceToBeAddedId));
    });
    return button;
}
function createQuizLabelQuestionText() {
    const label = document.createElement("label");
    label.for = stepIds.InputQuizQuestion;
    label.innerText = "Textul intrebarii quiz-ului";
    return label;
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

function createQuizInputQuestion() {
    const input = document.createElement("input");
    input.id = stepIds.InputQuizQuestion;
    input.type = "text";
    input.classList.add("endOfRow");
    return input;
}

function createQuizChoicePanel(choiceNo) {
    const deleteChoice = document.createElement("h1");
    deleteChoice.innerText = "🗑";
    deleteChoice.classList.add("quizChoiceDeleteIcon");
    deleteChoice.dataset.id = choiceNo;
    deleteChoice.addEventListener('click', (event) => {
        const target = event.target;
        const choiceNo = target.dataset.id;
        document.getElementById(`divChoice${choiceNo}`).remove();
    });
    
    const inputChoiceText = document.createElement("input"); 
    inputChoiceText.type = "text"; 
    inputChoiceText.placeholder = "Scrie textul raspunsului";
    inputChoiceText.id = `inputChoice${choiceNo}`;
    
    const addChoice = document.createElement("h1");
    addChoice.innerText = "☑";
    addChoice.title = "Marcheaza raspunsul ca fiind corect";
    addChoice.classList.add("quizChoiceCorrectIcon");
    addChoice.dataset.id = choiceNo;
    
    addChoice.addEventListener("click", (event) => {
        const target = event.target;
        const choiceNo = target.dataset.id;
        
        const inputChoice = document.getElementById(`inputChoice${choiceNo}`);
        inputChoice.classList.add("quizCorrectChoiceInputHighlight");
        inputChoice.dataset.isright = "true";
    });

    const div = document.createElement("div");
    div.classList.add("endOfRow");
    div.id = `divChoice${choiceNo}`;
    
    div.appendChild(deleteChoice);
    div.appendChild(inputChoiceText);
    div.appendChild(addChoice);
    return div;
}

function GetCourseId() {
    const panelCreateCourse = document.getElementById("panelCourseCreate");
    return parseInt(panelCreateCourse.dataset.id);
}