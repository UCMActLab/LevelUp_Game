TODO: EXPLAIN TO THE PLAYER WHAT THEY HAVE DONE, FEEDBACK ON THEIR DECISIONS

TODO These variables are just for the prototype. We need a function to calculate the mood trends, but we need more items in the database for that. 

== game_ending ==
// Final reaction of group 1
~ temp group1_speaker = LIST_RANDOM(LIST_ALL(group1_members))
~ temp group2_speaker = LIST_RANDOM(LIST_ALL(group2_members))
~ temp group3_speaker = LIST_RANDOM(LIST_ALL(group3_members))

TODO IF THE PLAYER HAS NOT FORWARDED ANY NEWS, THE FAMILY SHOULD BE CHECKING ON THEM - AND OTHER GROUPS SHOULD NOTE THAT THE PLAYER HAS BEEN VERY QUIET.


Has leído {news_read} noticias de las {news_count} que has recibido. {news_read == news_count: Está genial que prestes tanta atención.} {news_read == 0: Ya se vé que pasas un poco del asunto.} #parrafo
Has enviado {news_fake} noticias falsas a otros. {news_fake == news_count: No solo te lo crees todo, sino que lo extiendes a otros. Probablemente estés causando problemas a los tuyos con esa conducta.} #parrafo
{   
    - group_1 <= -2:
    {group1_speaker}: Creo que no ves las cosas como son. No me fío de ti.  
    - group_1 >= 2:
    {group1_speaker}: No hagas caso de lo que te digan otros. El mundo está lleno de mentirosos. 
  - else:
    {group1_speaker}: Espero que estéis todos bien.  
}

// Final reaction of group 2
{   
    - group_2 <= -2:
    {group2_speaker}: Nos tienes preocupados. Nos has mandado artículos que son bulos y no queremos que te engañen.
    - group_2 >= 2:
    {group2_speaker}: Nos hace ilusión que nos mandes artículos útiles. Parece que estás al día y tienes cuidado con las fuentes.
  - else:
    {group2_speaker}: ¿Cuándo te vamos a ver?
}

// Final reaction of group 3

{   
    - group_3 <= -2:
    {group3_speaker}: Ten cuidado con lo que compartes. Hay cosas que no parecen de confianza. 
    - group_3 > 2:
    {group3_speaker}: Sigue mandándonos cosas, tus artículos nos han sido muy útiles. 
  - else:
    {group3_speaker}: ¡Qué bueno hace hoy! Deberíamos guardar los teléfonos y dar un paseo. 
    }

Has llegado al final. #parrafo
GRACIAS POR JUGAR #parrafo -> END
