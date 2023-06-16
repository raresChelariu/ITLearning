import {
    GetCoursesWithSqlScripts, SetButtonOnClickHandlers
} from "/js/SqlPlayground/Playground.js";

SetButtonOnClickHandlers();

GetCoursesWithSqlScripts("/api/course/author/withscripts");
