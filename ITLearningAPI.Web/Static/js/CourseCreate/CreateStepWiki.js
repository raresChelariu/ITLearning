import {FetchHttpPostJson} from "/js/Fetcher.js";
import {AddStepToSummary} from "/js/CourseCreate/CourseCreateSummary.js";

const stepIds = {
    InputWikiTitle : "inputWikiTitle",
    InputWikiContent: "inputWikiContent",
    WikiContentPreview: "WikiPreview"
};

export function GetWikiStepBuilder() {
    const labelWikiTitle = createLabelWikiTitle();
    const inputWikiTitle = createInputWikiTitle();
    const wikiText = createInputWikiText();
    const buttonUpdatePreview = createButtonUpdatePreview();
    const contentPreview = createWikiContentPreview();
    const buttonAddStep = createButtonAddStep();
    
    return [labelWikiTitle, inputWikiTitle, wikiText, buttonUpdatePreview, contentPreview, buttonAddStep];
}

function createButtonUpdatePreview() {
    const button = document.createElement("button");
    button.innerText = "Afiseaza previzualizarea continutului";
    button.addEventListener("click", () => {
        const wikiContent = document.getElementById(stepIds.InputWikiContent).value;
        const preview = document.getElementById(stepIds.WikiContentPreview);
        console.log(wikiContent);
        preview.innerHTML = marked.parse(wikiContent);
        hljs.highlightAll();
    });
    button.classList.add("NextStep");
    return button;
}

function createWikiContentPreview() {
    const div = document.createElement("div");
    div.id = stepIds.WikiContentPreview;
    div.classList.add("endOfRow");
    return div;
}

function createLabelWikiTitle() {
    const labelTitle = document.createElement("label");
    labelTitle.setAttribute("for", stepIds.InputWikiTitle);
    labelTitle.innerText = "Titlu Wiki";
    return labelTitle;
}

function createInputWikiTitle() {
    const input = document.createElement("input");
    input.id = stepIds.InputWikiTitle;
    input.type = "text";
    input.classList.add("endOfRow");
    return input;
}

function createInputWikiText() {
    const input = document.createElement("textarea");
    input.id = stepIds.InputWikiContent;
    input.rows = 13;
    input.cols = 70;
    input.placeholder = "Introdu aici textul wiki-ului in format Markdown";
    input.maxLength = 512;
    input.classList.add("endOfRow");
    return input;
}

function createButtonAddStep() {
    const button = document.createElement("button");
    button.innerText = "Adauga pas";
    button.classList.add("NextStep");
    button.addEventListener("click", WikiCreateRequest);
    return button;
}

function GetCourseId() {
    const panelCreateCourse = document.getElementById("panelCourseCreate");
    return parseInt(panelCreateCourse.dataset.id);
}

function WikiCreateRequest() {
    const wikiTextElement = document.getElementById(stepIds.InputWikiContent);
    const inputWikiTitle = document.getElementById(stepIds.InputWikiTitle);
    
    const body = {
        courseWikiText: wikiTextElement.value,
        courseId: GetCourseId(),
        title: inputWikiTitle.value
    };
    
    FetchHttpPostJson("/api/coursewiki", body)
        .then(() => {
            alert("Pasul Wiki a fost creat cu succes!");
            AddStepToSummary({
               stepTitle: body.title 
            });
        })
        .catch(err => {
            alert("Pasul Wiki nu a putut fi creat!");
            console.log(err);
        });
}
