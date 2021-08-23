using RabbitMQ.Client;
using System;
using System.Text;

namespace Publisher
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

        string msg = " Bem-vindo ao RabbitMQ"; //Mensagem a ser posta na fila

        var body = Encoding.UTF8.GetBytes(msg);

        channel.BasicPublish(exchange:"", //Publicamos a msg informando a fila e o coporo da msg
                             routingKey:"saudacao_1",
                             basicProperties:null,
                             body:body);

        Console.WriteLine($" [X] Mensagem Enviada. \t Mensagem: {msg}");

        Console.ReadLine();

      }
    }
  }
}
