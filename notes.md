### FRAMEWORK

- Regras => Funções que transformam o elemento em questão, tanto para o mesmo 
tipo da entrada quanto para tipos diferentes. São classificadas em:
  - Regras: funções diversas não categorizadas. 
  - Condições: Regras que consideram a aplicabilidade de uma outra regra, retornando
  especificamente um boleano.
  - Efeitos: Regras que retornam o mesmo objeto, efetivamente o mutando.
  - Ações: Regras que geram novos eventos.
- Eventos => Objetos que encapsulam dados e um contexto específico para o acontecimento
de algum fato, de modo que múltiplos consumidores possam reagir a esse.
- Atores => Objetos especiais registrados no DI (nuvem), podendo serem Persistentes
(Singletons) ou Imediatos (Transients).

- Dados => Persistência
