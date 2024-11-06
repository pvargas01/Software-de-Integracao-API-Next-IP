import 'package:flutter/material.dart';
import '../models/contato.dart';
import 'adicionar_contato_screen.dart';
import 'editar_contato_screen.dart';

class ContatosScreen extends StatefulWidget {
  @override
  _ContatosScreenState createState() => _ContatosScreenState();
}

class _ContatosScreenState extends State<ContatosScreen> {
  List<Contato> contatos = []; // Lista de contatos vazia

  @override
  void initState() {
    super.initState();
    _loadContatos(); // Inicializa os dados de contato
  }

  void _loadContatos() {
    // Pode ser alterado para carregar dados estáticos, se necessário
    setState(() {});
  }

  void _adicionarContato(Contato contato) {
    setState(() {
      contatos.add(contato);
    });
  }

  void _editarContato(int index, Contato contatoEditado) {
    setState(() {
      contatos[index] = contatoEditado;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Contatos'),
        actions: [
          IconButton(
            icon: Icon(Icons.add),
            onPressed: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => AdicionarContatoScreen(onContatoAdicionado: _adicionarContato),
                ),
              );
            },
          ),
        ],
      ),
      body: ListView.builder(
        itemCount: contatos.length,
        itemBuilder: (context, index) {
          return ListTile(
            title: Text(contatos[index].nome),
            subtitle: Text(contatos[index].numeroWhatsApp),
            trailing: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                IconButton(
                  icon: Icon(Icons.edit),
                  onPressed: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (context) => EditarContatoScreen(
                          contato: contatos[index],
                          onContatoEditado: (contatoEditado) => _editarContato(index, contatoEditado),
                        ),
                      ),
                    );
                  },
                ),
                IconButton(
                  icon: Icon(Icons.delete),
                  onPressed: () {
                    setState(() {
                      contatos.removeAt(index); // Remover o contato da lista
                    });
                  },
                ),
              ],
            ),
          );
        },
      ),
    );
  }
}
