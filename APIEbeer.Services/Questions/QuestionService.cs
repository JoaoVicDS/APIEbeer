namespace APIEbeer.Services.Questions
{
    public class QuestionService : IQuestionService
    {
        private readonly Dictionary<string, string> _questionMappings = new(StringComparer.OrdinalIgnoreCase)
        {
            // === Características Originais ===
            { "Amargura", "Qual nível de amargor você prefere?" },
            { "Doçura", "Qual nível de doçura você prefere?" },
            { "Teor Alcoólico", "Qual o teor alcoólico que você prefere?" },
            { "Sabor", "Qual o sabor que você prefere?" },
            { "Textura", "Qual a textura que você prefere?" },
            { "Cor", "Qual a cor que você prefere?" },
            { "Carbonatação", "Qual o nível de carbonatação que você prefere?" },
            { "Ponto", "Qual o ponto da carne que você prefere?" },
            { "Temperatura", "Qual a temperatura que você prefere?" },
            { "Tempero", "Qual o tempero que você prefere?" },
            { "Acidez", "Qual o nível de acidez que você prefere?" },
            { "Molho", "Você prefere com molho ou sem molho?" },
            { "Ardência", "Qual o nível de ardência você prefere?" },
            { "Tamanho", "Qual o tamanho que você prefere?" },
            { "Volume", "Qual o volume que você prefere?" },

            // === Características do Burger King (4 FALTANTES ADICIONADAS AQUI) ===
            { "Proteína", "Qual tipo de proteína você prefere?" },
            { "Picância", "Qual seu nível de tolerância à pimenta?" },
            { "Adicionais", "Qual adicional não pode faltar?" },
            { "Tamanho da Carne", "Quanta fome você está? Escolha o tamanho do carne." },
            
            // === Características Universais (Já presentes, validadas) ===
            { "Tipo de Pão", "Qual seu tipo de pão favorito?" },
            { "Tipo de Queijo", "Que tipo de queijo te agrada mais?" },
            { "Molhos Especiais", "Qual molho especial chama sua atenção?" },
            { "Vegetais", "Qual vegetal fresco você prefere?" },
            { "Base da Sobremesa", "Qual a base perfeita para sua sobremesa?" },
            { "Cobertura", "Qual cobertura te dá mais água na boca?" },
            { "Mix-Ins", "O que você gosta de misturar na sua sobremesa?" },
            { "Tipo de Batata", "Como você prefere sua batata?" },
            { "Método de Preparo", "Qual método de preparo te parece mais saboroso?" },
            { "Estilo Culinário", "Qual estilo de culinária você está a fim hoje?" },
            { "Perfil de Sabor", "Que perfil de sabor você busca?" },
            { "Sensação", "Qual sensação você mais valoriza?" },
            { "Nível de Refrescância", "O quão refrescante sua bebida deve ser?" },
            { "Ideal para", "Qual a ocasião de hoje?" },
            { "Nível de Fome", "Defina sua fome em uma palavra:" },
            { "Tipo de Combo", "Você quer só o sanduíche ou o combo completo?" },
            { "Exclusividade", "Você busca um clássico ou uma novidade?" },
            { "Temperatura da Bebida", "Como você prefere a temperatura da sua bebida?" },
            { "Nível de Cafeína", "Precisa de um gás ou prefere relaxar?" },
            { "Tipo de Carne (Geral)", "Sua preferência é carne ou frango?" },
            { "Acompanhamento Principal", "Para acompanhar, vai de Batata Frita ou Onion Rings?" },
            { "Culinária", "De qual cozinha do mundo você está com vontade?" },
            { "Restrição Alimentar", "Você possui alguma restrição alimentar?" },
            { "Técnica de Cocção", "Qual técnica de preparo mais te agrada?" },
            { "Nível de Gordura", "Como você prefere o nível de gordura do seu prato?" },
            { "Intensidade do Sabor", "Você prefere um sabor mais suave ou mais intenso?" },
            { "Finalização", "Qual tipo de finalização te parece mais apetitosa?" },
            { "Tipo de Massa", "Qual tipo de massa você prefere para seu prato?" },
            { "Origem do Ingrediente", "Você valoriza a origem dos ingredientes?" },
            { "Corpo da Bebida", "Como você prefere o corpo da sua bebida?" },
            { "Harmonização", "Seu prato principal será qual destes?" },
            { "Base Alcoólica", "Qual a sua base alcoólica preferida para drinks?" },
            { "Perfil do Coquetel", "Qual o perfil de sabor ideal para o seu coquetel?" },
            { "Tipo de Grão (Café)", "Qual a origem do grão do seu café?" },
            { "Método de Extração (Café)", "Como você prefere que seu café seja preparado?" },
            { "Torra do Café", "Qual o nível de torra do seu café?" },
            { "Ocasião", "Para qual ocasião é a sua refeição?" },
            { "Tamanho da Porção", "A porção é para quantas pessoas?" },
            { "Formato de Serviço", "Qual formato de serviço você está procurando?" },
            { "Complexidade", "Que nível de complexidade você espera?" }
        };

        public string GetQuestionForCharacteristic(string characteristicName)
        {
            _questionMappings.TryGetValue(characteristicName, out var question);
            return question ?? string.Empty; 
        }
    }
}