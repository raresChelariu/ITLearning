export function BuildCourseCard(data) {
    const courseId = data["id"];
    
    const courseTitle = document.createElement("h2");
    courseTitle.innerText = data["name"];

    const description = document.createElement("p");
    description.innerText = "Some Course Description Here";

    const container = document.createElement("div")
    container.classList.add("card-content");
    container.appendChild(courseTitle);
    container.appendChild(description);

    const course = document.createElement("article");
    course.classList.add("card");
    course.appendChild(container);
    course.dataset.id = courseId;
    course.addEventListener("click", () => {
        window.location.replace(`/course/${courseId}`);
    });
    return course;
}