=== checkin_2 ===
~ temp group1_speaker = LIST_RANDOM(LIST_ALL(group1_members))
~ temp group2_speaker = LIST_RANDOM(LIST_ALL(group2_members))
~ temp group3_speaker = LIST_RANDOM(LIST_ALL(group3_members))

{article_forwarded_group1 == 0: {group1_speaker}: No nos cuentas nada, ¿eh?}
{article_forwarded_group2 == 0: {group2_speaker}: ¿Estás bien?}
{article_forwarded_group3 == 0: {group3_speaker}: Hace rato que no te vemos por el chat. }

Has leído {news_read} noticias de las {news_count} que has recibido. {news_read == news_count: Muy bien por examinar la información, recluta.} {news_read == 0: Recluta, recuerda que hay que tomarse este entrenamiento en serio.} 
#parrafo
Has enviado {news_fake} noticias falsas a otros. {news_fake == news_count: Recluta, tu misión es evitar que se extienda la desinformación, no contribuir a ella. } #parrafo

{s3_g3: Omar: Te quiero dar las gracias por enviar el artículo sobre inmigración. Le ha sido de ayuda a mi amiga para conseguir cita y así poder conseguir su permiso de trabajo.}

TODO Fix the conditional below, so the story reflects whether the mask articles have been sent or not. 
{s4_g1: María: Marcos está en el hospital. Se tomó la medicina del artículo que enviaste para ponerse bien más rápido, pero se pasó con la dosis y le han salido unas ronchas tremendas. Los médicos le van a tener en observación esta noche.} 

-> scene_5_intro
