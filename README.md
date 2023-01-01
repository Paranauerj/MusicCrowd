# Crowd Musician
O poder do Crowdsourcing na assistência de produtores musicais com deficiência visual.

### O Projeto
Este projeto destina-se ao auxílio de pessoas com deficiência visual na busca por material musical para composição de obras autorais, recorrendo aos recursos da Crowd.

A plataforma utilizada é o .NET Core Console Application, onde o utilizador abre o programa, solicita informação por voz e recebe, também, por voz, os resultados enviados pela crowd do Amazon MTurk.

Para um funcionamento correto do sintetizador e reconhecedor de voz, é necessário instalar o idioma en-US no Windows, em Definições -> Hora e Idioma -> Idioma -> Adicionar um  Idioma -> Inglês (Estados Unidos) e adicionar todas as opções.

#### Tech Stack
| Projeto| Versão|
|--|--|
| .NET Framework | v6.0 |
| System.Speech | v7.0.0 |
| Microsoft.EntityFrameworkCore | v7.0.0 |
| AWSSDK.MTurk | v3.7.100.20 |
| Newtonsoft.Json | v13.0.2 |
| NAudio | v2.1.0 |
