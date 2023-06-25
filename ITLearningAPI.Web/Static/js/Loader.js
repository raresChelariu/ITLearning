export function CreateLoader() {
    const element = document.createElement("div");
    element.classList.add("loader");
    element.id = "loader";
    return element;
}

export function RemoveLoader() {
    const element = document.getElementById("loader");
    element.remove();
}