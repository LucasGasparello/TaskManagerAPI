# Task Manager

O projeto da API foi implementado em .NET utilizando o MongoDB como banco de dados e docker compose, portanto o projeto pode ser iniciado com o comando.
docker-compose up --build

A estrutura do banco de dados está separado em duas collections, o project (projeto) e task (tarefa), cada tarefa tem o id do project vinculado e pode ter uma lista de comentários e uma lista de alterações. Todos endpoints seguem a regra de negocio apresentada nos requisitos do projeto.

Na segunda fase, as minhas perguntas para o PO seriam:
  - Quais funcionalidades adicionais os usuários considerariam essenciais para melhorar a usabilidade da API?
  - Existe algum feedback recorrente dos usuários que deveria ser priorizado?
  - Quais são as expectativas de volume de dados (número de usuários, tarefas, projetos) que a API deve suportar?(essa pergunta é essencial pois pode definir a necessidade de refatorar as estruturas do banco de dados, pois como o MongoDB limita os documentos até 16MB, nenhum documento pode ser muito grande, então caso tenha um grande volume de dados nas tarefas, por exemplo, seria ideal criar collections separadas para comentários e atualizações da tarefa, vinculados pelo ID da tarefa)
  - Existem requisitos específicos para tempo de resposta ou disponibilidade?
  - Quais serviços externos devem ser integrados à API ?
  - Será implementado autenticação ou privilégios de usuários?
  - Quais são as prioridades para os próximos ciclos de desenvolvimento?
  - Existem prazos ou entregas específicas que precisamos considerar?

  Pontos de melhoria identificados e propostas para elevar a qualidade técnica e funcional da API:
  - Melhorar a documentação existente no Swagger, com possibilidade de utilizar outras ferramentas, como scalar.
  - Avaliar a migração para uma arquitetura baseada em microserviços caso seja necessário outros serviços, como autenticação e CRUD do usuário
  - Adicionar taxas de limite de requisição (rate limiting) para evitar abuso e ataques DDoS
  - Adotar ferramentas de monitoramento (Grafana,Prometheus) para acompanhar a saúde do sistema em produção.
  - Implementar um sistema de logs (ELK Stack ou AWS CloudWatch) para facilitar a identificação de problemas.

  Como o projeto está utilizando um banco não relacional como o MongoDB é altamente escalável caso seja necessário adicionar mais serviços ou atributos em cada documento e como a regra de negocio permite apenas 20 tarefas por projeto poderia ser simplificado para apenas uma coleção contendo o projeto e uma lista de tarefas porém foi desenvolvido já pensando na utilização expandida, considerando várias tarefas por projeto.