import sys

if len(sys.argv) > 1:
    try:
        # Recebe o número passado como argumento
        tamanho = int(sys.argv[1])
        
        # Gera uma lista de strings com o mesmo tamanho do número
        array = [f"Elemento {i + 1}" for i in range(tamanho)]
        
        # Converte o array para uma string, unindo os elementos com vírgulas
        print(",".join(array))
    except ValueError:
        print("Erro: O parâmetro passado não é um número válido.")
else:
    print("Nenhum parâmetro foi passado.")
