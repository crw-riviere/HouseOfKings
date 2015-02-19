$(function () {
    var game = $.connection.game; // the generated client-side hub proxy
    var $btn = $('.action-pick-card');
    var $groupCircle = $('.group-circle');
    var groupName = null;
    function init() {
        groupName = $('#group-name').val();
        game.server.joinGroup(groupName);

        $(document).on('click', '.action-pick-card', function () {
            $btn.loadingButton({ text: 'Picking Card...' });
            game.server.pickCard(groupName).done(function () {
                $btn.text('Waiting for my turn...');
            });
        });
    }

    function distributePlayers() {
        var radius = 130;
        var players = $('.player');
        width = $groupCircle.width(), height = $groupCircle.height(),
        angle = 0, step = (2 * Math.PI) / players.length;
        players.each(function () {
            var x = Math.round(width / 2 + radius * Math.cos(angle) - $(this).width() / 2);
            var y = Math.round(height / 2 + radius * Math.sin(angle) - $(this).height() / 2);
            if (window.console) {
                console.log($(this).text(), x, y);
            }
            $(this).css({
                left: x + 'px',
                top: y + 'px'
            });
            angle += step;
        });
    }

    function parseSuit(suit) {
        $('.suit').removeClass('suit-heart suit-diamond suit-spade suit-club');

        switch (suit) {
            case 0: {
                $('.suit').addClass('suit-club');
                break;
            }
            case 1: {
                $('.suit').addClass('suit-diamond');
                break;
            }
            case 2: {
                $('.suit').addClass('suit-heart');
                break;
            }
            case 3:
            default: {
                $('.suit').addClass('suit-spade');
                break;
            }
        }
    }

    function parseNumber(number) {
        switch (number) {
            case 1: {
                number = 'A';
                break;
            }
            case 11: {
                number = 'J';
                break;
            } case 12: {
                number = 'Q';
                break;
            }
            case 13: {
                number = 'K';
                break;
            }
            default: {
                break;
            }
        }

        return number;
    }

    function updateStats(card) {
        $('#card-count').html(card.cardCount);
        $('#king-count').html(card.kingCount);
    }

    function updateCard(card) {
        var number = null;
        parseSuit(card.suit);
        number = parseNumber(card.number);
        $('.suit').html(number);
        console.log(card);
    }

    game.client.addPlayer = function (playerUsername) {
        game.client.setAudit(playerUsername + ' joined the game');
        $groupCircle.append('<div class="player"></div>');
        distributePlayers();
    }

    game.client.setTurn = function () {
        $btn.loadingButton({ reset: true });
    }

    game.client.showPickedCard = function (card) {
        game.client.setAudit(card.player + ' picked ' + '<span class="suit"></span>');
        game.client.setMessage(card.player + ' picked ' + '<div style="display:inline;" class="suit"></div><br/><br/><strong>' + card.rule.title + '</strong><br/><br/>' + card.rule.desc);

        updateStats(card);
        updateCard(card);
    }

    game.client.showGameOver = function (card) {
        game.client.setAudit(card.player + ' picked ' + '<span class="suit"></span>');
        game.client.setMessage(card.player + ' picked ' + '<div style="display:inline;" class="suit"></div><br/><br/><strong>Game Over</strong><br/><br/>');

        updateStats(card);
        updateCard(card);
    }

    game.client.setAudit = function (message) {
        $('#audit').html(message);
    }

    game.client.setMessage = function (message) {
        $('#message').html(message);
    }

    // Start the connection
    $.connection.hub.start().done(init);
});