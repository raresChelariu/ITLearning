import {FetchHttpPostFormData} from "/js/Fetcher.js";
import {AddStepToSummary} from "/js/CourseCreate/CourseCreateSummary.js";

const idInputVideoStepTitle = "inputTitleStepVideo";

export function GetVideoStepBuilder() {
    const form = document.createElement("form");
    
    form.id = "formStepVideo";
    form.setAttribute("enctype", "multipart/form-data");
    form.setAttribute("method", "POST");
    form.onsubmit = (form) => {
        VideoCreateRequest(form.submitter);
        return false;
    };
        
    form.appendChild(createLabelTitle());
    form.appendChild(createInputTitle());
    form.appendChild(createInputVideo());
    form.appendChild(createFormSubmit());

    return form;
}

function createLabelTitle() {
    const labelTitle = document.createElement("label");
    labelTitle.setAttribute("for", idInputVideoStepTitle);
    labelTitle.innerText = "Titlu Video";
    return labelTitle;
}

function createInputTitle() {
    const inputTitle = document.createElement("input");
    inputTitle.id = idInputVideoStepTitle;
    inputTitle.classList.add("endOfRow");
    return inputTitle;
}

function createInputVideo() {
    const inputVideo = document.createElement("input");
    inputVideo.setAttribute("type", "file");
    inputVideo.classList.add("endOfRow");
    return inputVideo;
}

function createFormSubmit() {
    const submit = document.createElement("input");
    submit.type = "submit";
    submit.value = "Adauga pas";
    submit.classList.add("NextStep");
    return submit;
}

async function VideoCreateRequest(oFormElement) {
    const formData = new FormData(oFormElement);
    
    const title = document.getElementById(idInputVideoStepTitle);
    const panelCreateCourse = document.getElementById("panelCourseCreate");
    
    formData.append("title", title.innerText);
    formData.append("courseId", panelCreateCourse.dataset.id);
    
    FetchHttpPostFormData("/api/video/upload", formData)
        .then(() => {
            alert("Pasul Video a fost creat cu succes!");
            AddStepToSummary({
                stepTitle: title.value
            });
        })
        .catch(err => {
            alert("Pasul Video nu a putut fi creat!");
            console.log(err);
        });
}