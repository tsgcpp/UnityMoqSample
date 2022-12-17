# Unity用Moq生成ツール

## Pluginsフォルダの配置方法
Pluginsフォルダの配置は以下を参考にしてください。

![image](https://user-images.githubusercontent.com/19503967/208157980-9371e01a-8ca5-41df-ba6e-df9ff80f5a91.png)


## Bashを使用する場合
```bash
# unzipをインストール (インストール済の場合は省略可能、aptや他のインストールコマンドでも化)
$ brew install unzip

$ cd <generate_moq_plugins.shがあるディレクトリ>

$ bash generate_moq_plugins.sh
```

コマンド完了後に`PluginGenerationToolOutput`ディレクトリにUnity用にMoqを構成したディレクトリが生成されます。  
Unityプロジェクトの "Api Compatibility Level" に合った方を使用してください。

### Api Compatibility Level が .NET Standard 2.1 の場合
`PluginGenerationToolOutput/netstandard2.1` を使用

### Api Compatibility Level が .NET Framework の場合
`PluginGenerationToolOutput/net462` を使用
