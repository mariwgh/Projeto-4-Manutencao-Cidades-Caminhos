using System;
using System.Collections.Generic;

public interface IQueue<Tipo>
{
  void Enfileirar(Tipo novoDado);
  Tipo Retirar();
  Tipo OInicio();
  Tipo OFim();
  bool EstaVazia { get; }
  int Tamanho    { get; }
  List<Tipo> Listar();
}
