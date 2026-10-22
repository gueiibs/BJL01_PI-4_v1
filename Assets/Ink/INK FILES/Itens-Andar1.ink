=== 1_salao_principal ===

= paredenorte_indicasecretaria
Alí é a secretaria.
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
Talvez esse mapa seja útil pra mim.
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
Não preciso usar o banheiro agora, mas talvez tenha algo útil lá.
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

= paredeleste_passagemcorredorprofs
Posso ir ver o que tem alí.
+ [Ir] # sala: 0
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
    Você entra. # sala: 1
    -> END
}
{ not TemItem("chavedasecretaria"):
    Está trancada.
    -> END
}
A chave encaixa na fechadura.
+ [Entrar]
    ~ porta_secretaria_aberta = true
    Você entra. # sala: 1
    -> END
+ [Ainda não]
    -> END   
= paredesul_cactosecretaria
{ TemItem("chavedasecretaria"):
    Parece não ter mais nada aqui.
- else:
    Um cacto... parece ter alguma coisa no vaso.
}
-> END
/* ---------------------------------
= alternativa c/ menos caracteres
    paredesul_cactosecretaria
{ not TemItem("testechave"):
    Um cacto... parece ter alguma coisa no vaso.
}
-> END

 ----------------------------------*/


=== zoom_cactosecretaria ===
= chavedasecretaria
Quem diria, uma chave. # pegar: chavedasecretaria
-> END
=== zoom_mapaquadro ===
= testeentramapa
Eu deveria ficar com isso.
-> END
=== zoom_recadosecretaria ===
= recadosecretaria
Parece ser um recado, dos bravos. # pegar: recadosecretaria
-> END
=== zoom_relogiosecretaria ===
= relogiosecretaria
Hmm...
-> END

=== zoom_computadorsecretaria ===
= computadorsecretaria
{ computadorsecretaria_desbloqueado:
    Posso dar uma olhada agora.
    -> END
}
-> END
= verificarsenha
{ senhacomputador == "1715":
    ~ computadorsecretaria_desbloqueado = true
    Posso dar uma olhada agora. # zoom: zoom_compdesbloqueado
- else:
    Senha incorreta. Tente novamente.
}
-> END

=== zoom_compdesbloqueado ===

= arquivo_1
Alguns relatórios escolares.
-> END

= arquivo_2
A lista dos alunos...
Talvez tenha algo aqui.
-> END

= arquivo_3
Um e-mail do diretor.
-> END

= lixeiracomputador
Está vazia.
-> END