public class Vertice
{
  string rotulo;
  bool foiVisitado;

  public Vertice(string rotulo)
  {
    this.rotulo = rotulo;
    foiVisitado = false;
  }
  public string Rotulo => rotulo;
  public bool FoiVisitado {  get => foiVisitado; set => foiVisitado = value; }
}

