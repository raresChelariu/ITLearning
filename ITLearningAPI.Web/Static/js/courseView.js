const courseId = GetCourseId(); 
console.log(courseId);

function GetCourseId()
{
    const url = window.location.href;
    const tokens = url.split('/');
    return parseInt(tokens[tokens.length - 1]);
}