using APIEbeer.Shared.ViewModels.JSON;

namespace APIEbeer.Services.Json
{
    public class JsonService : IJsonService
    {
        public (bool IsValid, string? ErrorMessage) IsValidJsonStructure(JsonViewModel? input)
        {
            if (input == null)
                return (false, "Objeto JSON nulo.");

            if (string.IsNullOrWhiteSpace(input.Restaurant))
                return (false, "Campo 'restaurant' é obrigatório.");

            if (input.Menu == null)
                return (false, "Campo 'menu' é obrigatório e não pode ser vazio.");

            foreach (var category in input.Menu.Categories)
            {
                if (category == null)
                    return (false, "O menu deve conter a chave 'category'.");

                if (category.Items == null || category.Items.Count < 0)
                    return (false, $"A categoria '{category.Name}' deve conter ao menos um item.");

                foreach (var item in category.Items)
                {
                    if (string.IsNullOrWhiteSpace(item.Name))
                        return (false, $"Cada item deve conter a chave 'name' na categoria '{category.Name}'.");

                    if (item.Characteristics == null || !item.Characteristics.Any())
                        return (false, $"O item '{item.Name}' deve conter ao menos uma característica.");

                    foreach (var characteristic in item.Characteristics)
                    {
                        if (string.IsNullOrWhiteSpace(characteristic.Name))
                            return (false, $"O item '{item.Name}' possui uma característica sem nome.");

                        if (string.IsNullOrWhiteSpace(characteristic.Value))
                            return (false, $"A característica '{characteristic.Name}' do item '{item.Name}' não possui um valor.");

                        if (characteristic.Options == null || !characteristic.Options.Any())
                            return (false, $"A característica '{characteristic.Name}' do item '{item.Name}' não possui opções.");
                    }
                }
            }

            return (true, null); // Estrutura válida
        }
    }
}
