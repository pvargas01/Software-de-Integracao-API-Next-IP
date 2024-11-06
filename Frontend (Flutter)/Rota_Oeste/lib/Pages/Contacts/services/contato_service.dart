// lib/services/contato_service.dart
/*
import 'dart:convert';
import 'package:http/http.dart' as http;
import '../models/contato.dart';

class ContatoService {
  final String baseUrl = 'https://seu-backend.com/api/contatos'; // Substitua pelo seu URL

  Future<List<Contato>> obterContatos() async {
    final response = await http.get(Uri.parse(baseUrl));
    if (response.statusCode == 200) {
      List<dynamic> data = json.decode(response.body);
      return data.map((item) => Contato.fromJson(item)).toList();  // Usando fromJson para criar a instância
    } else {
      throw Exception('Falha ao carregar contatos');
    }
  }

  Future<void> adicionarContato(Contato contato) async {
    await http.post(
      Uri.parse(baseUrl),
      headers: {'Content-Type': 'application/json'},
      body: json.encode(contato.toJson()),  // Usando toJson para converter a instância
    );
  }

  Future<void> editarContato(Contato contatoAntigo, Contato contatoNovo) async {
    await http.put(
      Uri.parse('$baseUrl/${contatoAntigo.id}'),  // Usando o ID do contato antigo
      headers: {'Content-Type': 'application/json'},
      body: json.encode(contatoNovo.toJson()),  // Usando toJson para converter a nova instância
    );
  }

  Future<void> removerContato(Contato contato) async {
    await http.delete(
      Uri.parse('$baseUrl/${contato.id}'),  // Usando o ID do contato
    );
  }
}
*/