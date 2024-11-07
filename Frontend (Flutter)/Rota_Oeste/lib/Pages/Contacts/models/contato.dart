class Contato {
  final String id;  // Identificador único do contato
  final String nome;  // Nome do contato
  final String numeroWhatsApp;  // Número do WhatsApp do contato

  // Construtor
  Contato({
    required this.id,
    required this.nome,
    required this.numeroWhatsApp,
  });

  // Método para converter o contato em um mapa
  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'nome': nome,
      'numeroWhatsApp': numeroWhatsApp,
    };
  }

  // Método estático para criar um contato a partir de um mapa
  factory Contato.fromJson(Map<String, dynamic> json) {
    return Contato(
      id: json['id'],
      nome: json['nome'],
      numeroWhatsApp: json['numeroWhatsApp'],
    );
  }
}
