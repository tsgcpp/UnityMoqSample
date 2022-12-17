# Unity用Moq生成ツール

## WARNING
以下のスクリプトは [MIT License](../LICENSE.md) として公開しています。

作者または著作権者は、ソフトウェアに関してなんら責任を負いません。

各モジュールのライセンスは [LICENSE.md](./LICENSE.md) をご確認ください。

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

- "Api Compatibility Level"が".NET Standard 2.1"の場合は`PluginGenerationToolOutput/netstandard2.1`を使用
- "Api Compatibility Level"が".NET Framework"の場合は`PluginGenerationToolOutput/net462`を使用

## GitHub Acrtionsを使用する場合
### 1. UnityMoqSample リポジトリをFork
[tsgcpp/UnityMoqSample](https://github.com/tsgcpp/UnityMoqSample) をForkしてください。

![image](https://user-images.githubusercontent.com/19503967/208222990-f9e0969c-b6d8-48ae-87e5-f2f606d3609f.png)

### 2. Action "Generate Moq Plugins" を実行
1. **Forkした方のUnityMoqSample**のトップページに行き、タブ "Actions" をクリック
2. Actionsページの左側の "Generate Moq Plugins" をクリック
3. `Run Workflow` をクリック
4. 吹き出しの `Run Workflow` (緑のボタン) をクリック
5. Actionの完了を待つ (早ければ1分もかかりません)

![image](https://user-images.githubusercontent.com/19503967/208223302-5eab2d58-67a4-4135-be0b-060ee5cc6de7.png)

### 3. Api Compatibility Levelに合わせてArtifactをダウンロード
完了するとzipファイルとしてダウンロード可能となります。

- "Api Compatibility Level"が".NET Standard 2.1"の場合は "Moq for Unity (.NET Standard 2.1)" をダウンロードして使用
- "Api Compatibility Level"が".NET Framework"の場合は "Moq for Unity (.NET Framework)" をダウンロードして使用

※ダウンロード期限は生成後2日に設定しています。

![image](https://user-images.githubusercontent.com/19503967/208224240-1a5fda0d-eec3-4649-bb07-e0cd6565baa1.png)
