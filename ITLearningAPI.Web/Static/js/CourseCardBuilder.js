export function BuildCourseCard(data) {
    const courseId = data["id"];
    
    const courseTitle = document.createElement("h2");
    courseTitle.classList.add("card-course-title");
    courseTitle.innerText = data["name"];

    const description = document.createElement("p");
    description.innerText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus fringilla feugiat ultricies. Vestibulum porttitor, mi sit amet dictum luctus, leo.";
    description.classList.add("card-course-description");
    
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