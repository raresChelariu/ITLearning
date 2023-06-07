import {FetchHttpPostFormData} from "../Fetcher";

const idInputVideoStepTitle = "inputTitleStepVideo";

export function ShowVideoStepBuilder() {
    const form = document.createElement("form");
    form.id = "formStepVideo";
    form.setAttribute("enctype", "multipart/form-data");
    form.setAttribute("method", "POST");
    form.onsubmit = (form) => {
        VideoCreateRequest(form.submitter);
        return false;
    };
    
    const labelTitle = document.createElement("label");
    const inputTitle = document.createElement("input");
    inputTitle.id = idInputVideoStepTitle;

    labelTitle.setAttribute("for", idInputVideoStepTitle);
    labelTitle.innerText = "Titlu Video";

    const inputVideo = document.createElement("input");
    inputVideo.setAttribute("type", "file");

    form.appendChild(labelTitle);
    form.appendChild(inputTitle);
    form.appendChild(inputVideo);
    return form;
}

async function VideoCreateRequest(oFormElement) {
    const formData = new FormData(oFormElement);
    
    const title = document.getElementById(idInputVideoStepTitle);
    const panelCreateCourse = document.getElementById("panelCourseCreate");
    
    formData.append("title", title.innerText);
    formData.append("courseId", panelCreateCourse.dataset.id);
    
    FetchHttpPostFormData("/api/video/upload", formData)
        .then(() => {
            alert("Pasul video a fost creat cu succes!");
        })
        .catch(err => {
            console.log(err);
        });
}