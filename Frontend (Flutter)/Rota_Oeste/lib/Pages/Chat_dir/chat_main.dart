import 'package:flutter/material.dart';
import 'package:flutter_chat_ui/flutter_chat_ui.dart';
import 'package:flutter_chat_types/flutter_chat_types.dart' as types;
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'dart:math';

class ChatMain extends StatefulWidget {
  const ChatMain({super.key});

  @override
  State<ChatMain> createState() => _ChatPageState();
}

class _ChatPageState extends State<ChatMain> {
  final List<types.Message> _messages = [];
  final _user = const types.User(id: '82091008-a484-4a89-ae75-a22bf8d6f3ac');

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        centerTitle: true,
        title: const Text(
          'Chat',
          style: TextStyle(fontSize: 30),
        ),
      ),
      body: Stack(
        children: [
          if (_messages.isEmpty)
            const Center(
              child: Text(
                'Chat',
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
    // Gera um ID único para a mensagem
    final messageId = DateTime.now().millisecondsSinceEpoch.toString() + Random().nextInt(1000).toString();

    final textMessage = types.TextMessage(
      author: _user,
      createdAt: DateTime.now().millisecondsSinceEpoch,
      id: messageId,
      text: message.text,
    );

    _addMessage(textMessage);

    await sendMessage("5565996220491", message.text); // Envia a mensagem digitada pelo usuário
  }

  void _addMessage(types.Message message) {
    setState(() {
      _messages.insert(0, message);
    });
  }

  Future<void> sendMessage(String to, String messageBody) async {
    final url = Uri.parse('http://localhost:5000/api/whatsapp/send');
    final headers = {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer EAB7uOlnCeBEBO3i7F91ZATVpnpje0iJhsy7eFIKInJzRGmngORSvCZC6TrPykR74fCClfq5lBLZBEB7TanCORXJhXWfH325VN3ZAkJLrbr42NMG9N4Au3QOSRddwqZBGJukDIWhBpZAwolNHIvMAXK8PXTybgbd2fbPvEiVVOMdYrFuL17Hc3wNSwKnJDoG2FZAmShruYC4PMZAJ8ljCZAw9IrYR3gqpT',
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
      }
    } catch (e) {
      print('Erro: $e');
    }
  }
}