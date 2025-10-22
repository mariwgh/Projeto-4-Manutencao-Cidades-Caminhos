using System.IO;

public interface IRegistro
{
  void LerRegistro(BinaryReader arquivo, long qualRegistro);
  void GravarRegistro(BinaryWriter arquivo);
  int TamanhoRegistro { get; }
}
