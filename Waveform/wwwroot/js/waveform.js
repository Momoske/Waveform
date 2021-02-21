function createWave(songs, pos, baseUrl) {

    var wavesurfer = WaveSurfer.create({
        progressColor: 'rgba(68, 141, 190, 0.99)',
        cursorColor: 'rgba(68, 141, 190, 0.66)',
        waveColor: 'rgba(68, 141, 190, 0.33)',
        container: '#waveformWave',
        barRadius: 3,
        barWidth: 3,
        height: 40,
        barGap: 1
    });

    wavesurfer.setVolume(0.1);
    loadSongAjax(baseUrl + songs[pos].getAttribute('aria-label'));

    wavesurfer.on('ready', function () {

        wavesurfer.play();
        $('#waveformInfo').html(songs[pos].querySelector('.songs__track__title > span > span').innerHTML);
        $('#waveformInfo').attr('title', songs[pos].querySelector('.songs__track__title > span > span').innerHTML);

        // Hiding and showing with display flex/none makes buttons not work after changing song
        $('#waveformCtrl').append(`
            <span class="material-icons-round" id="stop">stop</span>
            <span class="material-icons-round" id="prev">skip_previous</span>
            <span class="material-icons-round" id="play">pause_circle_outline</span>
            <span class="material-icons-round" id="next">skip_next</span>
            <span class="material-icons-round" id="volume">${wavesurfer.getMute() ? 'volume_off' : 'volume_up'}</span>
        `);

        $('#waveformCtrl').css('display', 'flex');

        $('#stop').on('click', () => wavesurfer.destroy());
        $('#next').on('click', () => wavesurfer.seekTo(0.999)); // 1 restarts the song?

        $('#prev').on('click', function () {
            if (wavesurfer.getCurrentTime() > 5 || pos === 0) {
                wavesurfer.seekTo(0);
            } else {
                pos -= 2;
                wavesurfer.seekTo(0.999);
                !wavesurfer.isPlaying() && wavesurfer.play();
            }
        });

        $('#play').on('click', function () {
            wavesurfer.playPause();
            $(this).html(wavesurfer.isPlaying() ? 'pause_circle_outline' : 'play_circle_outline');
        });

        $('#volume').on('click', function () {
            wavesurfer.toggleMute();
            $(this).html(wavesurfer.getMute() ? 'volume_off' : 'volume_up');
        });
    });

    wavesurfer.on('finish', function () {
        $('#play').html('play_circle_outline');
        if (pos < songs.length - 1) {
            pos++;
            $('#waveformCtrl').empty();
            $('#waveformInfo').html('');
            loadSongAjax(baseUrl + songs[pos].getAttribute('aria-label'));
        } else wavesurfer.destroy();
    });

    wavesurfer.on('destroy', function () {
        $('#waveformCtrl').empty();
        $('#waveformInfo').html('');
    });

    return wavesurfer;

}

function loadSongAjax(url) {
    return $.ajax({
        url: url,
        success: function (song) {
            loadSong(song);
        },
        error: function (err) {
            console.log('ERROR: ' + err);
        }
    });
}

function loadSong(song) {
    wavesurfer.load('data:audio/*;base64,' + song.audio);
}