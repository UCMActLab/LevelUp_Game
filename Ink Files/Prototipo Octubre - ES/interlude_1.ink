TODO REFRESHER TO PLAYERS TO READ THE NEWS AND THAT SENDING FAKES NEWS HAS CONSEQUENCES

=== checkin_1 === 
~ temp group1_speaker = LIST_RANDOM(LIST_ALL(group1_members))
~ temp group2_speaker = LIST_RANDOM(LIST_ALL(group2_members))
~ temp group3_speaker = LIST_RANDOM(LIST_ALL(group3_members))

{article_forwarded_group1 == 0: {group1_speaker}: Hace tiempo que no te vemos por el chat, ¿sigues ahí? }
{article_forwarded_group2 == 0: {group2_speaker}: ¿Estás bien? No sabemos de ti.}
{article_forwarded_group3 == 0: {group3_speaker}: Ya no nos mandas nada, ¿todo bien?  }

TODO These variables are just for the prototype. We need a function to calculate the mood trends, but we need more items in the database for that. 
TODO Rather than reporting this in text, it'd be better to show it in a counter / bar on top of the screen, similar to what Reigns does. 

Has leído {news_read} noticias de las {news_count} que has recibido. { news_read == news_count: ¡Estás aprendiendo bien!} {news_read == 0: Tendrás que prestar algo más de atención.} #parrafo
Has enviado {news_fake} noticias falsas a otros. {news_fake == news_count: ¡Hay que leer con más cuidado!} #parrafo

//The text below are the consequence for sending specific news to specific teams. This is hardcoded, so it can only be done with certain pieces of news. 
{s1_g3: Paula: He tenido que ir al banco. Mandé mis datos para recibir compensación por las pérdidad del apagón y me han robado los dados. ¡No utiliceis el link, es un timo!}
{s2_g1:
Marcos: Me voy a quedar en casa. Creo que cogí el COVID cuando fui al partido el otro día.
} 

TODO Consequences of sending the mask recommendation to friends

-> scene_3_intro