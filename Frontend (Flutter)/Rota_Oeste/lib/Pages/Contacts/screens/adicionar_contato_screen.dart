import 'package:flutter/material.dart';
import '../models/contato.dart';
import 'package:uuid/uuid.dart';

class AdicionarContatoScreen extends StatelessWidget {
  final TextEditingController nomeController = TextEditingController();
  final TextEditingController numeroController = TextEditingController();

  final Function onContatoAdicionado;

  AdicionarContatoScreen({required this.onContatoAdicionado});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Adicionar Contato'),
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
                // Validação básica dos campos
                if (nomeController.text.isEmpty || numeroController.text.isEmpty) {
                  ScaffoldMessenger.of(context).showSnackBar(
                    SnackBar(content: Text('Por favor, preencha todos os campos.')),
                  );
                  return;
                }

                final contato = Contato(
                  id: Uuid().v4(),  // Gerando um UUID como ID
                  nome: nomeController.text,
                  numeroWhatsApp: numeroController.text,
                );

                // Chama a função de callback para adicionar o contato
                onContatoAdicionado(contato);
                Navigator.pop(context); // Voltar para a tela anterior
              },
              child: Text('Adicionar'),
            ),
          ],
        ),
      ),
    );
  }
}
