## Implementação de índices utilizando um dataset

Projeto desenvolvido na disciplina de Estrutura de Dados com o objetivo de implementar índices em arquivo e memória utilizando determinado dataset. O conteúdo do dataset abrange 100 mil observações de estrelas, galáxias e quasares realizadas pelo SDSS (Sloan Digital Sky Survey). Cada observação traz os dados espectrais dos objetos.

Com base nas características do espectro, é possível descobrirmos se o objeto está se afastando ou aproximando de nós (efeito doppler). Também podemos chutar a sua velocidade relativa.

### Índices

Foram criados quatro índices: dois em arquivo e dois em memória. 

#### Índices em arquivo

Os índices em arquivo foram construídos com base nas colunas Alpha e Redshift. O atributo Alpha refere-se a ascensão reta do objeto. Já o atributo Redshift refere-se ao desvio para o vermelho, observado pelo SDSS.

Utilizando o índice de Redshift, é possível consultar quais objetos estão se aproximando de nós $(z<0)$ e quais estão se afastando de nós $(z>0)$. 

Com base no Redshift, também é possível calcular a velocidade relativa do objeto e descobrir qual objeto tem maior velocidade relativa. A velocidade relativa $v$ pode ser calculada através da fórmula
$$v\approx\left( \frac{(z+1)^2-1}{(z+1)^2+1}\right)\times velocidade\ da\ luz$$
Tanto para o índice de Redshift quanto para o índice de Alpha foram implementadas pesquisas específicas por determinado valor. Neste caso, como são números com muitas casas decimais, foi adicionada uma tolerância de 0.05. Para pesquisar o valor sobre o índice, foi utilizada a pesquisa binária.

#### Índices em memória

Os índices em memória utilizaram duas estruturas diferentes. O primeiro, baseado na Classe do objeto, é construído em uma lista. O segundo, baseado no atributo MJD (Modified Julian Date), é construído em uma árvore AVL.

A lista encadeada possibilita consultas utilizando apenas uma classe de objeto por vez. É possível fazer pesquisas utilizando apenas as estrelas, as galáxias ou os quasares.

A árvore AVL possibilita ordenar as datas. É fácil de encontrar o objeto mais antigo e mais recente registrados.
