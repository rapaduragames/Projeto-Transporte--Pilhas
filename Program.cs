using ProjetoTransporte.Models;
using ProjetoTransporte.Services;

namespace ProjetoTransporte;

internal class Program
{
    private static readonly TransporteService Transporte = new();

    private static void Main()
    {
        Transporte.ConfigurarDadosIniciais();

        int opcao;
        do
        {
            ExibirMenu();
            opcao = LerInteiro("Escolha uma opção: ", 0, 9);

            try
            {
                switch (opcao)
                {
                    case 1: CadastrarVeiculo(); break;
                    case 2: CadastrarGaragem(); break;
                    case 3: Transporte.IniciarJornada(); break;
                    case 4: Transporte.EncerrarJornada(); break;
                    case 5: LiberarViagem(); break;
                    case 6: ListarVeiculosGaragem(); break;
                    case 7: ConsultarQuantidadeViagens(); break;
                    case 8: ListarViagens(); break;
                    case 9: ConsultarPassageiros(); break;
                    case 0: Console.WriteLine("Programa finalizado."); break;
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Atenção: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Dados inválidos: {ex.Message}");
            }

            if (opcao != 0)
            {
                Console.WriteLine("\nPressione ENTER para continuar...");
                Console.ReadLine();
            }
        } while (opcao != 0);
    }

    private static void ExibirMenu()
    {
        Console.Clear();
        Console.WriteLine("======================================");
        Console.WriteLine("       SISTEMA DE TRANSPORTE");
        Console.WriteLine("======================================");
        Console.WriteLine($"Jornada: {(Transporte.JornadaAtiva ? "EM ANDAMENTO" : "ENCERRADA")}");
        Console.WriteLine("--------------------------------------");
        Console.WriteLine("0. Finalizar");
        Console.WriteLine("1. Cadastrar veículo");
        Console.WriteLine("2. Cadastrar garagem");
        Console.WriteLine("3. Iniciar jornada");
        Console.WriteLine("4. Encerrar jornada");
        Console.WriteLine("5. Liberar viagem");
        Console.WriteLine("6. Listar veículos em uma garagem");
        Console.WriteLine("7. Quantidade de viagens por origem/destino");
        Console.WriteLine("8. Listar viagens por origem/destino");
        Console.WriteLine("9. Passageiros transportados por origem/destino");
        Console.WriteLine("======================================");
    }

    private static void CadastrarVeiculo()
    {
        int capacidade = LerInteiro("Capacidade de passageiros do veículo: ", 1, 100);
        Veiculo veiculo = Transporte.CadastrarVeiculo(capacidade);
        Console.WriteLine($"Veículo {veiculo.Id} cadastrado com capacidade para {veiculo.Capacidade} passageiros.");
    }

    private static void CadastrarGaragem()
    {
        Console.Write("Nome da nova garagem/aeroporto: ");
        string nome = Console.ReadLine() ?? "";
        Transporte.CadastrarGaragem(nome);
        Console.WriteLine($"Garagem '{nome.Trim()}' cadastrada.");
    }

    private static void LiberarViagem()
    {
        string origem = EscolherGaragem("Origem");
        string destino = EscolherGaragem("Destino");
        Transporte.LiberarViagem(origem, destino);
    }

    private static void ListarVeiculosGaragem()
    {
        string garagem = EscolherGaragem("Garagem");
        Garagem local = Transporte.ObterGaragem(garagem);
        Console.WriteLine($"\nGaragem: {local.Nome}");
        Console.WriteLine($"Quantidade de veículos: {local.QuantidadeVeiculos}");
        Console.WriteLine($"Potencial de transporte: {local.PotencialTransporte} passageiros");

        if (local.VeiculosDoTopoAteOFundo().Count == 0)
        {
            Console.WriteLine("Nenhum veículo estacionado.");
            return;
        }

        Console.WriteLine("Veículos (primeiro da lista é o próximo a sair):");
        foreach (Veiculo veiculo in local.VeiculosDoTopoAteOFundo())
            Console.WriteLine($"- Veículo {veiculo.Id} | Capacidade: {veiculo.Capacidade}");
    }

    private static void ConsultarQuantidadeViagens()
    {
        (string origem, string destino) = EscolherRota();
        Console.WriteLine($"Viagens de {origem} para {destino}: {Transporte.QuantidadeViagens(origem, destino)}");
    }

    private static void ListarViagens()
    {
        (string origem, string destino) = EscolherRota();
        List<Viagem> viagens = Transporte.ListarViagens(origem, destino);

        Console.WriteLine($"\nViagens de {origem} para {destino}:");
        if (viagens.Count == 0)
        {
            Console.WriteLine("Nenhuma viagem registrada para esta rota.");
            return;
        }

        foreach (Viagem viagem in viagens)
            Console.WriteLine($"Veículo {viagem.VeiculoId} | {viagem.DataHora:dd/MM/yyyy HH:mm} | {viagem.Passageiros} passageiros");
    }

    private static void ConsultarPassageiros()
    {
        (string origem, string destino) = EscolherRota();
        Console.WriteLine($"Passageiros transportados de {origem} para {destino}: {Transporte.QuantidadePassageiros(origem, destino)}");
    }

    private static (string origem, string destino) EscolherRota()
    {
        string origem = EscolherGaragem("Origem");
        string destino = EscolherGaragem("Destino");
        return (origem, destino);
    }

    private static string EscolherGaragem(string titulo)
    {
        Console.WriteLine($"\n{titulo} - garagens disponíveis:");
        foreach (Garagem garagem in Transporte.Garagens)
            Console.WriteLine($"- {garagem.Nome}");

        Console.Write($"{titulo}: ");
        return Console.ReadLine() ?? "";
    }

    private static int LerInteiro(string mensagem, int minimo, int maximo)
    {
        while (true)
        {
            Console.Write(mensagem);
            if (int.TryParse(Console.ReadLine(), out int valor) && valor >= minimo && valor <= maximo)
                return valor;

            Console.WriteLine($"Digite um número entre {minimo} e {maximo}.");
        }
    }
}
