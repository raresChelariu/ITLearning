import {
    GetCoursesWithSqlScripts, SetButtonOnClickHandlers, PreviewQueryWithSyntaxHighlighting
} from "/js/SqlPlayground/Playground.js";

SetButtonOnClickHandlers();

GetCoursesWithSqlScripts("/api/course/author/withscripts");

PreviewQueryWithSyntaxHighlighting();