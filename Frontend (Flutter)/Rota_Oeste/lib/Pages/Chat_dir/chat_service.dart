import 'package:signalr_netcore/signalr_client.dart';

class ChatService {
  late HubConnection _connection;

  ChatService() {
    _initializeConnection();
  }

  void _initializeConnection() {
    _connection = HubConnectionBuilder()
        .withUrl("https://https://2a7a-200-129-242-4.ngrok-free.app/chathub") // ALTERAR PARA LINK CORRETO
        .build();

    // Escute as mensagens recebidas
    _connection.on("ReceiveMessage", (arguments) {
      if (arguments != null && arguments.isNotEmpty) {
        final message = arguments[0];

        if (message is Map<String, dynamic> &&
            message.containsKey('SenderId') &&
            message.containsKey('MessageText')) {
          final senderId = message['SenderId'];
          final messageText = message['MessageText'];
          print("Nova mensagem de $senderId: $messageText");

          // Adicione lógica para atualizar a interface do chat
          // Por exemplo, chamar um método no seu chat para adicionar a mensagem
        } else {
          print("Mensagem recebida não possui os campos necessários.");
        }
      } else {
        print("Argumentos recebidos são nulos ou vazios.");
      }
    });

    // Iniciar a conexão
    _connect();
  }

  Future<void> _connect() async {
    try {
      await _connection.start();
      print("Conexão com SignalR estabelecida!");
    } catch (e) {
      print("Erro ao conectar: $e");
    }
  }

  Future<void> sendMessage(String recipientId, String messageText) async {
    // Verifique se a conexão está ativa antes de tentar enviar a mensagem
    if (_connection.state == HubConnectionState.Connected) {
      try {
        await _connection.invoke("SendMessage", args: [recipientId, messageText]);
        print("Mensagem enviada para $recipientId: $messageText");
      } catch (e) {
        print("Erro ao enviar mensagem: $e");
      }
    } else {
      // Tenta reconectar caso não esteja conectado
      print("Não está conectado ao SignalR. Tentando reconectar...");
      await _connect();
      // Após reconectar, tente enviar a mensagem novamente
      await sendMessage(recipientId, messageText);
    }
  }
}
