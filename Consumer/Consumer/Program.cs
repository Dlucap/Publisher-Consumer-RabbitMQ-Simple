using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
{
  class Program
  {
    static void Main(string[] args)
    {

      var factory = new ConnectionFactory()
      {
        HostName = "localhost" // Definimos uma conexão com o nó RAbbitMQ em localhost
      };

      using (var connetcion = factory.CreateConnection()) //Abrir conexão com o RAbbitMQ
      using (var channel = connetcion.CreateModel()) /// Cria um canal onde será definido auma fila, uma mensagem e publicar  a mensagem
      {
        channel.QueueDeclare(queue: "saudacao_1", //Nome da fila
                             durable: false,// se true a fila permanece ativa após o serviço ser reiniciado
                             exclusive: false,//se ture ela so pode ser acessada via conexão atual e são excluidas ao fechar a conexão
                             autoDelete: false, // se true sera deletada automaticamente após o consumidor usar a fila
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel); // Solicita a entrega das mesg de forma  assincrona e fornece um retorno de chamada

        consumer.Received += (model, ea) =>
        {

          try
          {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body); // recebe a msg da fila e converte para string e imprime no console a mensagem
            Console.WriteLine($" [X] Mensagem Reebida: {message}");
            //channel.BasicAck(ea.DeliveryTag, false);
          }
          catch (Exception ex)
          {
            //log ex
            channel.BasicNack(ea.DeliveryTag, false, true);
          }
        };

        channel.BasicConsume(queue:"saudacao_1",
                             autoAck: true,// indicamos o consumo da msg
                             consumer: consumer);


        Console.ReadLine();

      }
    }
  }
}
//http://localhost:15672/#/ dashboard