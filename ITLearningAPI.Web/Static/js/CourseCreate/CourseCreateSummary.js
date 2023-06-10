
export function AddStepToSummary(data) {
    const summary = document.getElementById("stepsSummary");
    const step = CreateStepForSummary(data);
    summary.appendChild(step);
}

function CreateStepForSummary(data) {
    const step = document.createElement("a");
    step.innerText = data.stepTitle;
    return step;
}