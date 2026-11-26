=== 1_salao_principal ===

= paredenorte_indicasecretaria
Tem uma janela para uma sala alí.
-> END

= paredenorte_mapa
Acho que é um mapa, deveria olhar
-> END

= paredenorte_escadaandar2
Não posso subir sem olhar por aqui antes.
-> END

= paredenorte_hall
Não consigo descansar agora.
-> END

= paredeoeste_vidrosecretaria
Não parece ter ninguém lá agora, melhor eu dar uma olhada por aí.
-> END

= paredeoeste_passagementradaporao
Posso ir pra lá se eu quiser...
+ [Ir para lá] # sala: 2 //sala 2 = sala da entrada porao
    -> END 
+ [Ficar aqui]
    Melhor dar mais uma olhada por aí.
    -> END

= paredeleste_placaprofessores
Tá escrito "<b>APENAS AUTORIZADOS</b>" na placa.
-> END

= paredeleste_janeladiretor
Não dá pra ver nada com essa cortina na frente.
-> END

= paredeleste_mapaquadro
{ tem_mapanormal:
    Não preciso mais disso.
    -> END
}
Acho que é o mapa da escola, posso dar uma olhada.
+ [Verificar]
    Deixa eu ver direito. # zoom: zoom_mapaescolanormal
    -> END
+ [Agora não]
    Vou andar por aí antes.
    -> END

= paredesul_entradasaidaescola
Eu não devia sair até resolver <b>tudo isso</b>...
-> END

= paredesul_bebedouro
Não tô com sede.
-> END

=== 2_secretaria ===

= paredenorte_saidasecretaria
Não sei se quero sair daqui agora.
/*sala 2 = corredor entrada porao*/
+ [Sair agora] # sala: 2
    -> END 
+ [Ficar aqui]
    Talvez tenha mais alguma coisa aqui.
    -> END
    
= paredenorte_poster
<b>"Na campanha contra a violência"</b>.
-> END

= paredenorte_relogiosecretaria
Esse relógio tá estranho, parece parado.
-> END

= paredeoeste_plantasecretaria
{ TemItem("recadosecretaria"):
    A planta é bonita.
- else:
    Tem um papel atrás do vaso.
}
-> END

= paredeoeste_janelaclima
O tempo tá mudando, acho que vai chover.
-> END

= paredeleste_posterturmacaimare
"<b>Turma Caimaré, campeões da liga XXX</b>"... Que nome estranho pra uma turma.
-> END

= paredeleste_computadorsecretaria
O computador tá ligado, posso tentar dar uma olhada.
-> END

= paredeleste_posteres2
Mais posters sobre o colégio.
-> END

= paredesul_comododir
Tá trancado
-> END

= paredesul_comodoesq
Tá trancado
-> END

= paredesul_fotofilha
Que saudade de você, garotinha...
-> END

= paredesul_posters
Mais posters sobre o colégio.
-> END

= paredesul_ventilador
Esse ventilador tá todo empoeirado.
-> END


=== 3_entrada_porao ===

= paredenorte_banheiromasc
Não tô afim de usar o banheiro.
-> END

= paredenorte_banheirofem
Melhor eu não entrar aqui...
-> END

= paredenorte_plantaesq
Ela ama a natureza...
-> END

= paredenorte_plantadir
Isso me lembra dela...
-> END

= paredeoeste_quadrofundador
O fundador, esse quadro me dá arrepios.
-> END

= paredeoeste_placafundador
<b>"XXX, Fundador da XXX"</b>
-> END

= paredeoeste_alunomes
O aluno do mês, parece esforçado
-> END

= paredeoeste_painelfotos
Tem algumas fotos sobre quem estuda aqui.
-> END

= paredeoeste_painelnoticias
Tem as atualizações e noticias da semana.
-> END

= paredeleste_passagemcorredor
// 3 = corredor
Posso ir ver o que tem alí.
+ [Ir] # sala: 3
    -> END 
+ [Ficar aqui]
    Talvez tenha mais alguma coisa aqui.
    -> END
    
= paredeleste_passagemsalaoprincipal 
Eu posso voltar pro salão principal.
/*sala 0 = salao principal*/
+ [Voltar] # sala: 0
    -> END 
+ [Ficar aqui]
    Talvez tenha mais alguma coisa aqui.
    -> END
    
= paredesul_entradasecretaria
{ porta_secretaria_aberta:
    Melhor eu olhar aqui.
    -> END
}
{ not TemItem("chavedasecretaria"):
    Tá trancada.
    -> END
}
A chave parece ter encaixado.
+ [Entrar]
    ~ porta_secretaria_aberta = true
    Talvez tenha mais alguma coisa. # sala: 1
    -> END
+ [Ainda não]
    -> END   
= paredesul_plantasecretaria
{ TemItem("chavedasecretaria"):
    Parece não ter mais nada aqui.
- else:
    É uma planta bonita...
}
-> END

=== 6_corredor === 

= paredenorte_banco
Não tô afim de descansar agora.
-> END
= paredenorte_porta
Tá trancado, posso ver se tá na sala dos professores.
-> END

= paredeoeste_voltarsalaoprincipal
Posso voltar se eu quiser.
+ [Ir] # sala: 0 
    -> END 
+ [Ficar aqui]
    Melhor eu dar mais uma olhada aqui.
    -> END

= paredesul_janela
Não consigo ver nada lá dentro.
-> END

= paredesul_mapa
{ tem_mapanormal:
    Não preciso mais disso.
    -> END
}
Parece um mapa, posso ir lá ver.
-> END

= paredesul_passagem
Posso voltar se eu quiser.
+ [Ir] # sala: 0 
    -> END 
+ [Ficar aqui]
    Melhor eu dar mais uma olhada aqui.
    -> END

= paredeleste_mapa
{ tem_mapanormal:
    Não preciso mais disso.
    -> END
}
Acho que é o mapa da escola, posso dar uma olhada.
+ [Verificar]
    Deixa eu ver direito. # zoom: zoom_mapaescolanormal
    -> END
+ [Agora não]
    Vou andar por aí antes.
    -> END
    
= paredeleste_passagem
É a sala dos professores, pode ter algo importante pra lá.
+ [Ir] # sala: 5
    -> END 
+ [Ficar aqui]
    Melhor eu dar mais uma olhada aqui.
    -> END

/* ---------------------------------

=== 7_sala_zelador ===

= paredenorte_
= paredeoeste_
= paredesul_ 
= paredeleste_

 --------------------------------- */

=== 8_sala_professores ===

= paredenorte_mesadir
.
-> END
= paredenorte_mesaesq
.
-> END
= paredenorte_itens
.
-> END
= paredenorte_quadro
.
-> END
= paredeoeste_passagemsalaoprincipal
Posso voltar se eu quiser.
+ [Ir] # sala: 0 
    -> END 
+ [Ficar aqui]
    Melhor eu dar mais uma olhada aqui.
    -> END
-> END

= paredeoeste_quadro
.
-> END

= paredeoeste_mesa
.
-> END

= paredesul_sofa
.
-> END

= paredesul_poltrona
.
-> END

= paredesul_porta 
{ porta_secretaria_aberta:
    Melhor eu olhar aqui.
    -> END
}
{ not TemItem("chavediretoria"):
    Tá trancada.
    -> END
}
Que bom, a chave encaixou.
+ [Entrar]
    ~ porta_diretoria_aberta = true
    Talvez tenha mais alguma coisa. # sala: 6
    -> END
+ [Ainda não]
    Vou olhar por aí antes.
    -> END   
= paredeleste_quadroesq
.
-> END

= paredeleste_quadrodir
.
-> END

= paredeleste_meio
.
-> END

= paredeleste_mesa
.
-> END


=== 9_sala_diretor === 

= paredenorte_porta
Posso voltar pra lá se eu quiser.
+ [Ir] # sala: 5
    -> END 
+ [Ficar aqui]
    Melhor eu dar mais uma olhada aqui.
    -> END
= paredenorte_poltronadireita
Não posso descansar agora.
-> END

= paredenorte_poltronaesquerda
Não posso descansar agora.
-> END

= paredenorte_quadro
.
-> END

= paredeoeste_armario
.
-> END

= paredeoeste_janela
Não tenho motivo pra abrir aqui.
-> END

= paredesul_computador
Posso dar uma olhada.
-> END

= paredeleste_pratileiraleste
.
-> END

= paredeleste_escrivaninhaleste
.
-> END

= paredeleste_tocafitas
 Um toca fitas...
 + [Ver de perto] # zoom: zoom_tocafitas
    -> END 
+ [Ignorar]
    Agora não.
    -> END
 
 
 
 
 /* ----------------------- */
 
 
=== zoom_cactosecretaria ===
= chavedasecretaria
Quem diria, uma chave. # pegar: chavedasecretaria
-> END

=== zoom_mapaescolanormal ===
= mapaescolanormal
É o mapa da escola.
+ [Pegar]
    ~ tem_mapanormal = true
    Vai ser útil. # mapa
    -> END

=== zoom_recadosecretaria ===
= recadosecretaria
Parece ser um recado, dos bravos. # pegar: recadosecretaria
-> END
=== zoom_relogiosecretaria ===
= relogiosecretaria
Hmm... O relógio parece tá travado às 14:30.
-> END

=== zoom_computadorsecretaria ===
= computadorsecretaria
{ computadorsecretaria_desbloqueado:
    Posso dar uma olhada agora.
    -> END
}
-> END
= verificarsenha
{ senhacomputador == "1435":
    ~ computadorsecretaria_desbloqueado = true
    Posso dar uma olhada agora. # zoom: zoom_compdesbloqueado
- else:
    Senha incorreta. Tente novamente.
}
-> END

=== zoom_compemail ===

= clickpascoa
# zoom: zoom_emailpascoa
É só um aviso sobre a Páscoa.
-> END

= clickdesconhecido
# zoom: zoom_emaildesconhecido
...
Que porra é essa?
-> END

=== zoom_compdesbloqueado ===

= navegadorcomputador
Talvez tenha algo aqui.
# zoom: zoom_compemail
Alguns emails...
-> END

= arquivo_1
Alguns relatórios escolares.
-> END

= arquivo_2
A lista dos alunos...
Talvez tenha algo aqui, mas nada que eu precise agora.
-> END

= arquivo_3
Tá vazio.
-> END

= lixeiracomputador
Está vazia.
-> END