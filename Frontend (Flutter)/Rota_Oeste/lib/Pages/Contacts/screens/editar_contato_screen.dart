import 'package:flutter/material.dart';
import '../models/contato.dart';

class EditarContatoScreen extends StatelessWidget {
  final Contato contato;
  final Function onContatoEditado;

  EditarContatoScreen({required this.contato, required this.onContatoEditado});

  @override
  Widget build(BuildContext context) {
    final TextEditingController nomeController = TextEditingController(text: contato.nome);
    final TextEditingController numeroController = TextEditingController(text: contato.numeroWhatsApp);

    return Scaffold(
      appBar: AppBar(
        title: Text('Editar Contato'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          children: [
            TextField(
              controller: nomeController,
              decoration: InputDecoration(labelText: 'Nome'),
            ),
            TextField(
              controller: numeroController,
              decoration: InputDecoration(labelText: 'Número de WhatsApp'),
              keyboardType: TextInputType.phone,
            ),
            SizedBox(height: 20),
            ElevatedButton(
              onPressed: () {
                // Validação básica
                if (nomeController.text.isEmpty || numeroController.text.isEmpty) {
                  ScaffoldMessenger.of(context).showSnackBar(
                    SnackBar(content: Text('Por favor, preencha todos os campos.')),
                  );
                  return; // Não continuar se os campos estão vazios
                }

                final contatoEditado = Contato(
                  id: contato.id,  // Preservando o ID do contato original
                  nome: nomeController.text,
                  numeroWhatsApp: numeroController.text,
                );

                // Chama a função de callback para editar o contato
                onContatoEditado(contatoEditado);
                Navigator.pop(context); // Voltar para a tela anterior
              },
              child: Text('Salvar'),
            ),
          ],
        ),
      ),
    );
  }
}
