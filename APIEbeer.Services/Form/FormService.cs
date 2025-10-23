using APIEbeer.Services.Questions;
using APIEbeer.Shared.ViewModels.Form;
using APIEbeer.Shared.ViewModels.JSON;

namespace APIEbeer.Services.Form
{
    public class FormService(IQuestionService questionService) : IFormService
    {
        private readonly IQuestionService _questionService = questionService;

        public FormViewModel CreateDynamicForm(JsonViewModel model) // ALTERADO: Recebe o modelo completo
        {
            if (model?.Menu == null)
                throw new ArgumentNullException(nameof(model), "Model and Menu cannot be null.");

            var categories = CreateCategories(model.Menu.Categories, model.QuestionLimitPerCategory);

            if (categories == null || !categories.Any())
                throw new ArgumentException("Não foi possível gerar categorias com perguntas.");

            var formId = Guid.NewGuid().ToString();

            return new FormViewModel { FormId = formId, Categories = categories };
        }

        private List<FormCategoryViewModel> CreateCategories(List<CategoryViewModel> categories, int? questionLimit)
        {
            var questionsByCategory = new List<FormCategoryViewModel>();
            foreach (var category in categories)
            {
                var questionsForThisCategory = CreateQuestionsForCategory(category.Items, questionLimit);
                if (questionsForThisCategory.Any())
                {
                    questionsByCategory.Add(new FormCategoryViewModel
                    {
                        Name = category.Name,
                        Questions = questionsForThisCategory
                    });
                }
            }
            return questionsByCategory;
        }

        // MÉTODO RENOMEADO E CORRIGIDO para maior clareza.
        private List<FormQuestionsViewModel> CreateQuestionsForCategory(List<ItemViewModel> items, int? questionLimit)
        {
            // Junta TODAS as características de TODAS as cervejas
            var allCharacteristics = items
                .SelectMany(i => i.Characteristics)
                .GroupBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First()) // Mantém apenas uma por nome
                .OrderBy(_ => Guid.NewGuid()) // Embaralha
                .ToList();

            // Aplica o limite máximo, se houver
            var limit = questionLimit.HasValue && questionLimit.Value > 0
                ? Math.Min(questionLimit.Value, allCharacteristics.Count)
                : allCharacteristics.Count;

            var selectedCharacteristics = allCharacteristics
                .Take(limit)
                .ToList();

            var questions = new List<FormQuestionsViewModel>();
            foreach (var characteristic in selectedCharacteristics)
            {
                var questionText = _questionService.GetQuestionForCharacteristic(characteristic.Name);
                if (string.IsNullOrWhiteSpace(questionText)) continue;

                questions.Add(new FormQuestionsViewModel
                {
                    Characteristic = characteristic.Name,
                    Question = questionText,
                    Options = items
                        .SelectMany(i => i.Characteristics)
                        .Where(c => c.Name.Equals(characteristic.Name, StringComparison.OrdinalIgnoreCase))
                        .SelectMany(c => c.Options)
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList()
                });
            }
            return questions;
        }
    }
}