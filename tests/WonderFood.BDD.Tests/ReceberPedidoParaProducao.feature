Funcionalidade: Receber Pedido para iniciar Preparo
Como um funcionário da cozinha do restaurente
Eu desejo receber os Pedidos dos Clientes
Para que eu iniciar o preparo
    
Cenário: Pedido recebido com sucesso
    Dado que o Cliente efetua um pedido
    E esse Pedido possui pelo menos um produto
    Quando recebermos a comunicação do Pedido
    Então ele deve ser inserido na nossa base de dados para ser possível iniciar o preparo
    