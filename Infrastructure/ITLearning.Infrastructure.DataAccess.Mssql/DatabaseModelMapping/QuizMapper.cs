using ITLearning.Domain;
using ITLearning.Infrastructure.DataAccess.Mssql.DatabaseModels;

namespace ITLearning.Infrastructure.DataAccess.Mssql.DatabaseModelMapping;

internal static class QuizMapper
{
    public static Quiz FromQuizDbAndQuizChoiceDb(QuizDbDto quiz, IEnumerable<QuizChoiceDbDto> choices)
    {
        return new Quiz
        {
            CourseId = quiz.CourseId,
            ItemId = quiz.ItemId,
            QuestionText = quiz.QuestionText,
            PossibleAnswers = choices.Select(x => x.FromQuizChoiceDb()).ToList()
        };
    }

    private static QuizChoice FromQuizChoiceDb(this QuizChoiceDbDto choice)
    {
        return new QuizChoice
        {
            ChoiceText = choice.ChoiceText,
            QuizChoiceId = choice.QuizChoiceId,
            CorrectChoice = choice.CorrectChoice
        };
    }
}