import {FetchHttpPostFormData} from "/js/Fetcher.js";
import {AddStepToSummary} from "/js/CourseCreate/CourseCreateSummary.js";

const urlVideoUpload = "/api/video/upload";

const stepIds = {
    FormStepVideo: "formStepVideo",
    InputVideoTitle: "inputTitleStepVideo",
    InputVideoFile: "inputVideoFile",
}

export function GetVideoStepBuilder() {
    const emptyParagraph = document.createElement("p");
    const form = document.createElement("form");
    
    form.id = stepIds.FormStepVideo;
    form.setAttribute("enctype", "multipart/form-data");
    form.setAttribute("method", "post");
    form.addEventListener("submit", VideoCreateRequest);
    
    form.setAttribute("action", urlVideoUpload);
    
    form.appendChild(createLabelTitle());
    form.appendChild(createInputTitle());
    form.appendChild(emptyParagraph);
    form.appendChild(createLabelVideo());
    form.appendChild(createInputVideo());
    form.appendChild(emptyParagraph);
    form.appendChild(createFormSubmit());

    return form;
}

function createLabelTitle() {
    const labelTitle = document.createElement("label");
    labelTitle.setAttribute("for", stepIds.InputVideoTitle);
    labelTitle.innerText = "Titlu Video";
    return labelTitle;
}

function createInputTitle() {
    const inputTitle = document.createElement("input");
    inputTitle.id = stepIds.InputVideoTitle;
    inputTitle.setAttribute("name", "title");
    inputTitle.classList.add("endOfRow");
    return inputTitle;
}

function createLabelVideo() {
       
    const label = document.createElement("label");
    label.setAttribute("for", stepIds.InputVideoFile);
    label.innerHTML = "<svg fill=\"#FFFFFF\" xmlns=\"http://www.w3.org/2000/svg\" height=\"1em\" viewBox=\"0 0 640 512\">" 
        + "<path d=\"M144 480C64.5 480 0 415.5 0 336c0-62.8 40.2-116.2 96.2-135.9c-.1-2.7-.2-5.4-.2-8.1c0-88.4 71.6-160 160-160c59.3 0 111 32.2 138.7 80.2C409.9 102 428.3 96 448 96c53 0 96 43 96 96c0 12.2-2.3 23.8-6.4 34.6C596 238.4 640 290.1 640 352c0 70.7-57.3 128-128 128H144zm79-217c-9.4 9.4-9.4 24.6 0 33.9s24.6 9.4 33.9 0l39-39V392c0 13.3 10.7 24 24 24s24-10.7 24-24V257.9l39 39c9.4 9.4 24.6 9.4 33.9 0s9.4-24.6 0-33.9l-80-80c-9.4-9.4-24.6-9.4-33.9 0l-80 80z\"/>" 
        + "</svg>" 
        + "Incarca videoclipul";
    
    return label;
}

function createInputVideo() {
    const inputVideo = document.createElement("input");
    inputVideo.setAttribute("type", "file");
    inputVideo.setAttribute("name", "file");
    inputVideo.id = stepIds.InputVideoFile;
    return inputVideo;
}

function createFormSubmit() {
    const submit = document.createElement("input");
    submit.type = "submit";
    submit.value = "Adauga pas";
    submit.classList.add("NextStep");
    return submit;
}

async function VideoCreateRequest(event) {
    event.preventDefault();
    const form = document.getElementById(stepIds.FormStepVideo);
    const formData = new FormData(form);
    const panelCreateCourse = document.getElementById("panelCourseCreate");
    const courseId = panelCreateCourse.dataset.id;
    
    formData.append("courseId", courseId);
    
    FetchHttpPostFormData(urlVideoUpload, formData)
        .then(() => {
            alert("Pasul Video a fost creat cu succes!");
            AddStepToSummary({
                stepTitle: document.getElementById(stepIds.InputVideoTitle).value
            });
        })
        .catch(err => {
            alert("Pasul Video nu a putut fi creat!");
            console.log(err);
        });
}