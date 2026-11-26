# Projeto Transporte

Sistema de console em C# para controlar a movimentação de uma frota de vans entre garagens de aeroportos, utilizando **pilhas (`Stack<T>`)** para representar os estacionamentos.

## Requisitos

- .NET 6 SDK ou superior.
- Terminal ou Visual Studio.

## Como executar

No terminal, dentro da pasta do projeto:

```bash
dotnet restore
dotnet run
```

## Cenário inicial

O sistema começa com:
- 8 veículos, identificados de 1 a 8;
- capacidade inicial de 15 passageiros por veículo;
- duas garagens: Congonhas e Guarulhos.

Novos veículos e garagens podem ser cadastrados pelo menu, somente quando a jornada estiver encerrada.

## Funcionalidades

| Opção | Funcionalidade |
|---|---|
| 0 | Finalizar o programa |
| 1 | Cadastrar veículo e sua capacidade |
| 2 | Cadastrar garagem |
| 3 | Iniciar jornada e distribuir os veículos alternadamente |
| 4 | Encerrar jornada, emitir relatório e limpar os registros diários |
| 5 | Liberar viagem entre duas garagens |
| 6 | Listar veículos, quantidade e capacidade total disponível na garagem |
| 7 | Consultar quantidade de viagens por rota |
| 8 | Listar viagens por rota |
| 9 | Consultar passageiros transportados por rota |

## Regra da pilha

Cada garagem possui sua própria `Stack<Veiculo>`. Ao liberar uma viagem, o veículo é retirado do topo da pilha da origem (`Pop`) e colocado no topo da pilha do destino (`Push`). Assim, somente o veículo que está na posição de saída pode partir, respeitando a regra LIFO (Last In, First Out).

Quando a pilha da origem fica vazia, não é possível liberar outra viagem a partir dela até que um veículo retorne.

## Premissas adotadas

1. Cada viagem liberada ocorre com o veículo em sua capacidade máxima, pois o enunciado não prevê cadastro individual de passageiros ou controle de embarque.
2. A capacidade padrão dos oito veículos iniciais é de 15 passageiros. A capacidade pode ser definida no cadastro de novos veículos.
3. No início de cada jornada, todos os veículos são redistribuídos alternadamente entre as garagens cadastradas. Essa decisão representa a distribuição inicial diária solicitada no enunciado.
4. O histórico de viagens e os totais por veículo são diários. Ao encerrar a jornada, o relatório é exibido e os registros são zerados.
5. Os veículos permanecem cadastrados após o encerramento da jornada.

## Diagrama de classes

O arquivo `diagrama-classes.mmd` contém o diagrama em Mermaid. Para visualizar, abra o arquivo em um editor compatível com Mermaid ou cole seu conteúdo em https://mermaid.live.

## Estrutura

```text
ProjetoTransporte/
├── Models/
│   ├── Garagem.cs
│   ├── Veiculo.cs
│   └── Viagem.cs
├── Services/
│   └── TransporteService.cs
├── Program.cs
├── ProjetoTransporte.csproj
├── diagrama-classes.mmd
└── README.md
```
