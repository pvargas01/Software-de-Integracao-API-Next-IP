import 'package:flutter/material.dart';
import 'package:flutter_chat_ui/flutter_chat_ui.dart';
import 'package:flutter_chat_types/flutter_chat_types.dart' as types;
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'dart:math';
import 'package:signalr_netcore/signalr_client.dart';



class ChatPage extends StatefulWidget {
  const ChatPage({super.key});

  @override
  State<ChatPage> createState() => _ChatPageState();
}

class _ChatPageState extends State<ChatPage> {
  final List<types.Message> _messages = [];
  final _user = const types.User(id: '82091008-a484-4a89-ae75-a22bf8d6f3ac');
  late HubConnection _connection;

  @override
  void initState() {
    super.initState();
    _initializeSignalR();
  }

  Future<void> _initializeSignalR() async {
    _connection = HubConnectionBuilder()
        .withUrl("LINK-NROK/chathub") // ALTERAR PARA LINK CORRETO
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

          _addMessage(types.TextMessage(
            author: types.User(id: senderId),
            createdAt: DateTime.now().millisecondsSinceEpoch,
            id: DateTime.now().millisecondsSinceEpoch.toString(),
            text: messageText,
          ));
        } else {
          print("Mensagem recebida não possui os campos necessários.");
        }
      } else {
        print("Argumentos recebidos são nulos ou vazios.");
      }
    });

    try {
      await _connection.start();
      print("Conexão com SignalR estabelecida!");
    } catch (e) {
      print("Erro ao conectar: $e");
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        centerTitle: true,
        title: const Text(
          'Rota Oeste',
          style: TextStyle(fontSize: 30),
        ),
      ),
      body: Stack(
        children: [
          if (_messages.isEmpty)
            const Center(
              child: Text(
                'Rota Oeste',
                style: TextStyle(fontSize: 30, fontWeight: FontWeight.bold),
              ),
            ),
          Chat(
            messages: _messages,
            onSendPressed: _handleSendPressed,
            user: _user,
            theme: DefaultChatTheme(
              primaryColor: Colors.black,
              secondaryColor: Colors.grey[300]!,
              inputBackgroundColor: Colors.black,
              inputTextColor: Colors.white,
              sentMessageBodyTextStyle: const TextStyle(
                color: Colors.white,
              ),
              receivedMessageBodyTextStyle: const TextStyle(
                color: Colors.black,
              ),
            ),
          ),
        ],
      ),
    );
  }

  void _handleSendPressed(types.PartialText message) async {
    final messageId = DateTime.now().millisecondsSinceEpoch.toString() + Random().nextInt(1000).toString();

    final textMessage = types.TextMessage(
      author: _user,
      createdAt: DateTime.now().millisecondsSinceEpoch,
      id: messageId,
      text: message.text,
    );

    _addMessage(textMessage);

    // Enviar a mensagem
    await sendMessage("5565996220491", message.text);
  }

  void _addMessage(types.Message message) {
    setState(() {
      _messages.insert(0, message);
    });
  }

  Future<void> sendMessage(String to, String messageBody) async {
    final url = Uri.parse('https://2a7a-200-129-242-4.ngrok-free.app/api/whatsapp/send'); // ALTERAR PARA LINK CORRETO
    final headers = {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer EAB7uOlnCeBEBOzzpmC3eNZCXAwgOjz7ZA8B9E6ZCc2j18O27mz6uRezNOYvZA1WR6r9xPN2ErqTxcJWjF6ZAOsVTFjYy8sjdiZB690wEB5MAxA9AqCa6bxQIZBmlriU3D3EDC5CoRyWFOSUw2aDU20qR4PTfrgtAOETOxuDfWrwKJmrvRibW1HiGSuZAuSTRi3QbWB88Vi7xOX1lXZBIfYZB8ZAFz2qjtAZD',
    };

    final payload = {
      'to': to,
      'messageBody': messageBody,
    };

    try {
      final response = await http.post(
        url,
        headers: headers,
        body: jsonEncode(payload),
      );

      if (response.statusCode == 200) {
        print('Mensagem enviada com sucesso!');
      } else {
        print('Falha ao enviar mensagem. Status: ${response.statusCode}');
        print('Resposta: ${response.body}'); // Adicione essa linha para ver a resposta do servidor
      }
    } catch (e) {
      print('Erro: $e');
    }
  }
}