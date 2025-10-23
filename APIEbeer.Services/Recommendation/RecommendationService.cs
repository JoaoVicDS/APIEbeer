using APIEbeer.Services.Cache;
using APIEbeer.Shared.ViewModels.Answers;
using APIEbeer.Shared.ViewModels.Form;
using APIEbeer.Shared.ViewModels.JSON;
using APIEbeer.Shared.ViewModels.Recommendation;

namespace APIEbeer.Services.Recommendation
{
    public class RecommendationService(ICacheService cacheService) : IRecommendationService
    {
        private readonly ICacheService _cacheService = cacheService;

        public RecommendationViewModel CreateRecommendation(AnswersViewModel answers, List<CategoryViewModel> categories)
        {
            var originalForm = _cacheService.Get<FormViewModel>($"formId:{answers.FormId}:FormViewModel");
            if (originalForm == null)
                throw new InvalidOperationException("Formulário expirado ou não encontrado no cache.");

            if (!IsValidAnswers(answers, originalForm))
                throw new ArgumentException("As respostas fornecidas são inválidas para o formulário original.", nameof(answers));

            var recommendationCategories = new List<RecommendationCategoryViewModel>();

            foreach (var answersCategory in answers.Categories)
            {
                var category = categories.FirstOrDefault(c => c.Name.Equals(answersCategory.Name, StringComparison.OrdinalIgnoreCase));
                if (category == null)
                    continue;

                var recommendationCategory = CreateCategoryRecommendation(
                    answersCategory.Name,
                    answersCategory.SelectedAnswers,
                    category.Items);

                if (recommendationCategory != null)
                {
                    recommendationCategories.Add(recommendationCategory);
                }
            }

            return new RecommendationViewModel
            {
                Categories = recommendationCategories
            };
        }

        private RecommendationCategoryViewModel CreateCategoryRecommendation(
            string categoryName,
            List<SelectedAnswersViewModel> selectedAnswers,
            List<ItemViewModel> items)
        {
            var itemsScored = new Dictionary<string, float>();

            foreach (var item in items)
            {
                float currentItemScore = 0;
                foreach (var answer in selectedAnswers)
                {
                    var itemCharacteristic = item.Characteristics
                        .FirstOrDefault(c => c.Name.Equals(answer.CharacteristicAsked, StringComparison.OrdinalIgnoreCase));

                    if (itemCharacteristic == null)
                        continue;

                    if (itemCharacteristic.Value.Equals(answer.SelectedOption, StringComparison.OrdinalIgnoreCase))
                    {
                        currentItemScore += 1.0f;
                    }
                }
                itemsScored[item.Name] = currentItemScore;
            }

            var bestItems = GetBestItems(itemsScored);

            var recommendedItems = bestItems.Select(item => new RecommendationItemViewModel { Recommendation = item }).ToList();

            return new RecommendationCategoryViewModel
            {
                Name = categoryName,
                Items = recommendedItems,
            };
        }

        // MÉTODO OTIMIZADO com LINQ para eficiência e segurança.
        private List<string> GetBestItems(Dictionary<string, float> itemsScored)
        {
            if (itemsScored == null || !itemsScored.Any())
                return new List<string> { "Nenhuma Recomendação" };

            // Ordena os itens pela pontuação (do maior para o menor), pega os 3 primeiros e seleciona seus nomes.
            var bestItems = itemsScored
                .OrderByDescending(kv => kv.Value)
                .Take(1)
                .Select(kv => kv.Key)
                .ToList();

            return bestItems.Any() ? bestItems : new List<string> { "Nenhuma Recomendação" };
        }

        private bool IsValidAnswers(AnswersViewModel answers, FormViewModel originalForm)
        {
            foreach (var answerCategory in answers.Categories)
            {
                var originalCategory = originalForm.Categories.FirstOrDefault(c => c.Name == answerCategory.Name);
                if (originalCategory == null) return false;

                foreach (var selectedAnswer in answerCategory.SelectedAnswers)
                {
                    var originalQuestion = originalCategory.Questions.FirstOrDefault(q => q.Characteristic == selectedAnswer.CharacteristicAsked);
                    if (originalQuestion == null) return false;

                    if (!originalQuestion.Options.Contains(selectedAnswer.SelectedOption, StringComparer.OrdinalIgnoreCase))
                        return false;
                }
            }
            return true;
        }
    }
}