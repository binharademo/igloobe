# IGloobe - Sistema de Interação por Captura de Movimento

## Visão Geral do Projeto

O IGloobe é um aplicativo interativo desenvolvido em C# que utiliza tecnologia de captura de movimento via Bluetooth para permitir controle de apresentações e desenho em tela. O sistema foi projetado para oferecer uma experiência interativa similar ao Wiimote da Nintendo, mas adaptado para uso em apresentações e aplicações educacionais.

**Período de Desenvolvimento:** Julho de 2010 a Abril de 2011 (com base nas datas de modificação dos arquivos)

## Funcionalidades Principais

1. **Conexão com Dispositivo Bluetooth**
   - Descoberta automática de dispositivos compatíveis
   - Gerenciamento de conexão com estado
   - Reconexão automática em caso de perda de sinal

2. **Controle de Apresentações**
   - Navegação entre slides de apresentações PowerPoint
   - Suporte a arquivos .ppt e .pptx
   - Conversão automática para formato de apresentação (.pps)
   - Ocultação da barra de tarefas do Windows durante apresentações

3. **Desenho Interativo**
   - Quadro branco virtual
   - Desenho sobre a área de trabalho
   - Controle de cores (preto, azul, vermelho, amarelo, branco)
   - Controle de espessura do traço (3 níveis)
   - Controle de transparência
   - Ferramenta de apagamento

4. **Controle de Cursor**
   - Mapeamento do movimento do dispositivo para o cursor
   - Suporte a clique direito
   - Calibração para ajuste de sensibilidade

5. **Utilitários Adicionais**
   - Acesso rápido ao teclado virtual do Windows
   - Interface minimalista e não intrusiva

## Arquitetura do Sistema

O sistema é dividido em cinco projetos principais, cada um com responsabilidades específicas:

### 1. Connector (Biblioteca Base)
- Define interfaces e classes abstratas para comunicação com dispositivos
- Implementa o padrão State para gerenciar estados de conexão
- Fornece mecanismos de log e depuração
- Contém a lógica de calibração e transformação de coordenadas (warping)

### 2. IGloobeMote (Captura de Movimento)
- Implementa a comunicação com o dispositivo físico via HID (Human Interface Device)
- Processa dados de acelerômetro e sensor infravermelho
- Gerencia eventos de botões e movimentos
- Implementa a leitura e escrita na memória do dispositivo

### 3. ConnectorWindows (Implementação Windows)
- Implementa a comunicação Bluetooth específica para Windows
- Utiliza a biblioteca InTheHand.Net.Personal para Bluetooth
- Gerencia o ciclo de vida da conexão com o dispositivo

### 4. iGloobeAppsMain (Aplicações)
- Contém as interfaces gráficas para desenho e apresentação
- Implementa a lógica de negócio das aplicações
- Gerencia a interação do usuário com o sistema
- Implementa formulários arrastáveis e transparentes para melhor experiência do usuário

### 5. ConnectorMain (Aplicação Principal)
- Ponto de entrada do sistema
- Inicializa os componentes necessários
- Gerencia o ciclo de vida da aplicação
- Previne múltiplas instâncias do aplicativo

## Tecnologias Utilizadas

### Linguagens e Frameworks
- **C# (.NET Framework 2.0)** - Linguagem principal de desenvolvimento
- **Windows Forms** - Framework para interface gráfica

### Bibliotecas Externas
- **InTheHand.Net.Personal (32bits)** - Biblioteca para comunicação Bluetooth
- **Microsoft.VisualBasic.PowerPacks.Vs** - Componentes adicionais para Windows Forms

### APIs e Técnicas
- **P/Invoke** - Chamadas nativas para APIs do Windows (user32.dll)
- **HID (Human Interface Device)** - Protocolo para comunicação com o dispositivo
- **Windows Registry** - Para armazenamento de configurações

## Padrões de Design Implementados

1. **Singleton**
   - Utilizado nas classes principais de formulários (FormAppsMain, FormAppsDraw, FormAppsPresentation)
   - Garante uma única instância de componentes críticos

2. **State Pattern**
   - Implementado na classe ConnectorImpl para gerenciar os diferentes estados da conexão
   - Estados incluem: NotInitialized, SearchingBluetooth, BluetoothClientFound, ConnectingDevice, InitializingMotionCapture, ReadyToUse

3. **Factory Method**
   - Utilizado na classe Connector para criar instâncias específicas de implementação
   - Permite a substituição da implementação em tempo de execução

4. **Observer Pattern**
   - Implementado através de eventos para notificação de mudanças de estado
   - Utilizado para comunicação entre camadas do sistema

5. **Template Method**
   - Utilizado na classe ConnectionStateImpl para definir o fluxo de operações
   - Permite que subclasses implementem comportamentos específicos

## Estrutura de Diretórios

```
IGloobeConnector/
├── connector/                  # Biblioteca base
│   ├── src/                    # Código-fonte
│   │   ├── CalibrationData.cs  # Dados de calibração
│   │   ├── Commands/           # Comandos do sistema
│   │   ├── Connector.cs        # Classe abstrata base
│   │   ├── ConsoleManager.cs   # Gerenciamento de console
│   │   ├── Gui/                # Interfaces gráficas comuns
│   │   ├── Hardware.cs         # Abstração de hardware
│   │   ├── MotionCapture.cs    # Captura de movimento
│   │   └── Warper.cs           # Transformação de coordenadas
│   └── Properties/             # Propriedades do assembly
│
├── motionCapture/              # Implementação da captura de movimento
│   ├── src/                    # Código-fonte
│   │   ├── DataTypes.cs        # Tipos de dados
│   │   ├── HidBinder.cs        # Comunicação HID
│   │   ├── IGloobeMote.cs      # Implementação principal
│   │   └── StateChanged*.cs    # Eventos de mudança de estado
│   └── Properties/             # Propriedades do assembly
│
├── connectorWindows/           # Implementação específica para Windows
│   ├── src/                    # Código-fonte
│   │   ├── ConnectorImpl.cs    # Implementação do conector
│   │   └── Connector/          # Classes auxiliares
│   ├── dlls/                   # Bibliotecas externas
│   └── Properties/             # Propriedades do assembly
│
├── iGloobeAppsMain/            # Aplicações principais
│   ├── src/                    # Código-fonte
│   │   ├── DraggableForm.cs    # Formulário base arrastável
│   │   ├── FormAppsMain.cs     # Formulário principal
│   │   ├── FormAppsDraw.cs     # Aplicação de desenho
│   │   ├── FormAppsPresentation.cs # Aplicação de apresentação
│   │   ├── Window.cs           # Utilitários de janela
│   │   └── Windows.cs          # Gerenciamento de janelas
│   ├── img/                    # Recursos de imagem
│   └── Properties/             # Propriedades do assembly
│
└── connectorMain/              # Aplicação principal
    ├── src/                    # Código-fonte
    │   └── Start.cs            # Ponto de entrada
    └── Properties/             # Propriedades do assembly
```

## Detalhes Técnicos Importantes

### Comunicação Bluetooth
- Utiliza a biblioteca InTheHand.Net.Personal para comunicação Bluetooth
- Implementa descoberta de dispositivos e conexão automática
- Gerencia reconexão em caso de perda de sinal

### Captura de Movimento
- Processa dados do sensor infravermelho para determinar posição
- Utiliza algoritmo de suavização para reduzir tremores
- Implementa calibração para mapear coordenadas do sensor para a tela

### Transformação de Coordenadas (Warping)
- Implementa algoritmo de transformação de coordenadas para mapear o espaço do sensor para o espaço da tela
- Suporta calibração em quatro pontos para maior precisão
- Aplica suavização para melhorar a experiência do usuário

### Controle de Cursor
- Utiliza P/Invoke para chamar APIs nativas do Windows para controle do cursor
- Implementa simulação de cliques do mouse
- Suporta diferentes modos de operação (absoluto e relativo)

### Interface Gráfica
- Implementa formulários transparentes e arrastáveis
- Utiliza ícones e recursos visuais para melhor experiência do usuário
- Suporta diferentes modos de visualização (tela cheia, janela)

## Limitações e Considerações

1. **Compatibilidade**
   - Desenvolvido para .NET Framework 2.0, que é uma versão antiga
   - Pode requerer ajustes para funcionar em sistemas operacionais modernos
   - Utiliza bibliotecas de 32 bits, o que pode causar problemas em sistemas de 64 bits

2. **Segurança**
   - Não implementa criptografia na comunicação Bluetooth
   - Utiliza P/Invoke, o que pode representar riscos de segurança se não for usado corretamente

3. **Manutenibilidade**
   - Alguns valores estão hardcoded no código
   - Documentação interna limitada em algumas partes do código
   - Utiliza tecnologias legadas (Windows Forms)

4. **Testes**
   - Não há evidências de testes automatizados
   - Depende de testes manuais para validação

## Boas Práticas Implementadas

1. **Separação de Responsabilidades**
   - Cada projeto tem uma responsabilidade bem definida
   - Classes com responsabilidades únicas

2. **Uso de Interfaces**
   - Interfaces bem definidas para comunicação entre componentes
   - Facilita a substituição de implementações

3. **Tratamento de Erros**
   - Implementação de tratamento de exceções em pontos críticos
   - Logs para facilitar a depuração

4. **Modularidade**
   - Sistema dividido em módulos independentes
   - Facilita a manutenção e extensão

5. **Padrões de Design**
   - Uso adequado de padrões de design para resolver problemas comuns
   - Melhora a estrutura e organização do código

## Conclusão

O IGloobe é um aplicativo bem estruturado que demonstra boas práticas de programação, apesar de utilizar tecnologias mais antigas. A arquitetura do sistema é bem pensada, com separação clara de responsabilidades e uso adequado de padrões de design.

A implementação da comunicação Bluetooth e da captura de movimento é robusta, com tratamento adequado de erros e estados. A interface gráfica é minimalista e não intrusiva, proporcionando uma boa experiência ao usuário.

No entanto, o uso de tecnologias mais antigas (.NET Framework 2.0 e Windows Forms) pode limitar sua compatibilidade com sistemas operacionais mais recentes e dificultar a manutenção futura. A falta de testes automatizados também é um ponto de atenção.

Em resumo, o IGloobe representa um esforço significativo de desenvolvimento, com uma arquitetura bem pensada e implementação sólida, apesar das limitações tecnológicas da época em que foi desenvolvido.

## Requisitos do Sistema

- Sistema Operacional: Windows XP/Vista/7
- .NET Framework 2.0 ou superior
- Adaptador Bluetooth compatível
- Dispositivo de captura de movimento compatível (similar ao Wiimote)
- Resolução mínima de tela: 800x600

## Licença

Este projeto foi desenvolvido entre 2010 e 2011 e seus direitos autorais pertencem aos desenvolvedores originais.
