@Code
    ViewBag.Title = "コミュニティの参加"
End Code

<div class="row">
    <div class="col-sm-12 col-md-8">
        <img src="@Href("~/images/stamprally2015/stamprally-logo.png")" alt="" class="img-responsive" />

        <h1>コミュニティの募集</h1>

        <p>IT勉強会スタンプラリーに参加いただけるコミュニティを募集中です。</p>
        <p>参加コミュニティには、スタンプラリーの台紙を送ります。また、スポンサーのノベルティのプレゼントもあります（開催時期などにより用意できない場合もあります）。</p>

        <h3>コミュニティの参加条件</h3>
        <p>
            参加は無料ですが、スタンプもしくはシール（オリジナル・既製品問わず）はご用意ください。
        </p>
        <p>
            参加した場合、スタンプラリーの運営に関わる作業（IT勉強会の登録および報告、スタンプの押印など）を実施していただくことになります。
            詳細は、下記の資料でご確認ください。
        </p>

        <div style="margin:50px 0 50px;">
            <div style="width: 510px" id="__ss_12092582">
                <strong style="display: block; margin: 12px 0 4px">
                    <a href="http://www.slideshare.net/it-stamp/it-321"
                       title="IT 勉強会 スタンプラリー コミュニティ向け" target="_blank">IT 勉強会 スタンプラリー コミュニティ向け</a>
                </strong>
                <iframe src="https://www.slideshare.net/slideshow/embed_code/12092582?rel=0" width="510"
                        height="426" frameborder="0" marginwidth="0" marginheight="0" scrolling="no"></iframe>
                <div style="padding: 5px 0 12px">
                    View more <a href="http://www.slideshare.net/" target="_blank">presentations</a>
                    from <a href="http://www.slideshare.net/it-stamp" target="_blank">IT 勉強会スタンプラリー</a>
                </div>
            </div>
            <p>※ 資料内の<a href="http://it-stamp.jp/resources/manual.pdf">マニュアル</a></p>
        </div>

        <p>
            参加希望のコミュニティは、資料を確認いただいた上で <a href="">運営事務局</a> までお知らせください。
            参加検討されているコミュニティからの質問などもお待ちしています。
        </p>


    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>
