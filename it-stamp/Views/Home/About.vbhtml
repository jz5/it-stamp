﻿@Code
    ViewBag.Title = "IT勉強会スタンプについて"
End Code

<div class="row">
    <div class="col-sm-12 col-md-8">
        <h1>🔰 初めての方へ</h1>

        <h2>✅ IT勉強会にチェックイン！</h2>

        <p><strong>IT勉強会スタンプ</strong>は、<mark>IT勉強会の参加を記録できるWebサービス</mark>です。</p>
        <p>IT勉強会に参加したら「チェックイン」して記録しましょう。</p>

        <h2>📖 チェックインの仕方</h2>
        <ol>
            <li><a href="@Href("~/Events/")">IT勉強会一覧</a>から<mark>参加したIT勉強会を選択</mark>。</li>
            <li>「<mark>チェックイン</mark>」すれば完了！</li>
        </ol>

        <p>
            参加したIT勉強会が一覧にない場合は、あなたが「<a href="@href("~/Events/Add/")">新規登録</a>」してください。<br />
            新規登録は、開催地域・日時・勉強会の名前を入力するだけ！
        </p>
        <p class="text-muted">イベント登録サイトのIT勉強会情報からチェックインもできる予定です。</p>

        <h2>🗾 IT勉強会スタンプラリー 2015に参加しよう！</h2>
        <p>誰でも参加できる「<a href="@Href("~/Stamprally/2015/")">IT勉強会スタンプラリー 2015</a>」を開催中です。</p>
        <p><a href="@Href("~/Events/?SpecialEvent=1")">対象のIT勉強会</a>を探して、参加してみましょう！</p>

        <h2>🔧 その他の機能</h2>
        <h3>💞 チェックインの共有</h3>
        <p>チェックインした情報を共有できます。チェックインと同時にツイートしたり、参加したIT勉強会の一覧を共有したりできます。</p>

        <h3>📘 スタンプのコレクション（実装予定）</h3>
        <p>チェックインするとスタンプがもらえます。いろいろなIT勉強会のスタンプをコレクションして楽しめます。</p>

        <h3>💬 コメント</h3>
        <p>IT勉強会の情報にコメントできます。使われた資料のリンクをコメントしたり便利に使ってくださいね。</p>

        <h3>💓 フォロー</h3>
        <p>IT勉強会やコミュニティを「フォロー」できます。フォローしておくと、すばやく情報にアクセスできますよ。</p>


        <h2>IT勉強会を主催していますか？</h2>
        <p>IT勉強会を主催しているコミュニティの方は、詳細な情報を編集できます。</p>
        <p><a href="@Href("~/Stamprally/2015/")">IT勉強会スタンプラリー 2015</a> の参加コミュニティも募集中です。</p>

    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>
